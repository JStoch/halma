using backend.Services;
using HalmaServer.Models;
using HalmaServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Mono.TextTemplating;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalmaServer.Hubs
{
    //[Authorize] //Uncoment to provide authorization to asset
    public class GameHub : Hub
    {
        private GameService GameService { get; set; }
        private BotDispacherService BotService { get; set; }
        private readonly ILogger<GameHub> _logger;

        public GameHub(GameService gameService, BotDispacherService botDispacherService, ILogger<GameHub> logger) : base()
        {
            this.GameService = gameService;
            this.BotService = botDispacherService;
            this._logger = logger;
        }

        public async Task RequestNewGame(string playerGuid)
        {
            _logger.LogInformation("RequestNewGame called with playerGuid: {playerGuid}", playerGuid);

            var game = GameService.StartGameOrWait(playerGuid, Context.ConnectionId);
            if (game != null)
            {
                var player = game.GetPlayer(playerGuid);
                var oponnent = game.GetOponnent(playerGuid);
                _logger.LogInformation("Game started between player {playerGuid} and opponent {oponnentGuid}", playerGuid, oponnent.Guid);

                await Clients.Client(oponnent.ConnectionId).SendAsync("NewGame", game.Guid, game.GetPlayerSymbol(oponnent.Guid));
                await Clients.Caller.SendAsync("NewGame", game.Guid, game.GetPlayerSymbol(player.Guid));

                await SyncGameState(Context.ConnectionId, game);
                await SyncGameState(oponnent.ConnectionId, game);
            }
            else
            {
                _logger.LogInformation("Player {playerGuid} is waiting for a game", playerGuid);
                await Clients.Caller.SendAsync("WaitingForGame");
            }
        }

        public async Task RequestNewGameWithBot(string playerGuid, DifficulityLevel difficulityLevel = DifficulityLevel.EASY)
        {
            _logger.LogInformation("RequestNewGameWithBot called with playerGuid: {playerGuid}, difficultyLevel: {difficultyLevel}", playerGuid, difficulityLevel);

            var game = GameService.StartGameOrWait(playerGuid, Context.ConnectionId, requestBotOponent: true);
            if (game != null)
            {
                var player = game.GetPlayer(playerGuid);
                _logger.LogInformation("Game started with bot for player {playerGuid}", playerGuid);

                await Clients.Caller.SendAsync("NewGameWithBot", game.Guid, game.GetPlayerSymbol(player.Guid));

                await SyncGameState(Context.ConnectionId, game);
                BotService.InitForNewConnection(Context.ConnectionId, level: difficulityLevel);
            }
            else
            {
                _logger.LogInformation("Player {playerGuid} is waiting for a game with bot", playerGuid);
                await Clients.Caller.SendAsync("WaitingForGameWithBot");
            }
        }

        private async Task MakeMoveBot(GameModel game, string playerGuid, List<int> from, List<int> to)
        {
            _logger.LogInformation("MakeMoveBot called for player {playerGuid} in game {gameGuid}", playerGuid, game.Guid);

            (List<int> botFrom, List<int> botTo) = await BotService.RequestOpponentMove(Context.ConnectionId, from, to);
            var isMoveValid = GameService.MakeMove(game.Guid, game.Player2Guid, botFrom, botTo);

            if (!isMoveValid)
            {
                _logger.LogWarning("Bot move was invalid for game {gameGuid}", game.Guid);
                return;
            }

            await SyncGameState(Context.ConnectionId, game);

            if (game.IsGameFinished())
            {
                _logger.LogInformation("Game {gameGuid} finished. Player {playerGuid} won", game.Guid, playerGuid);
                await Clients.Caller.SendAsync("EndOfGame", game.DidPlayerWin(playerGuid));
                BotService.ClearConnectionIfExist(Context.ConnectionId);
            }
        }

        public async Task MakeMove(string gameGuid, string playerGuid, List<int> from, List<int> to)
        {
            _logger.LogInformation("MakeMove called for player {playerGuid} in game {gameGuid}", playerGuid, gameGuid);

            var isMoveValid = GameService.MakeMove(gameGuid, playerGuid, from, to);
            var game = GameService.GetGame(gameGuid);
            if (game == null)
            {
                _logger.LogWarning("Game {gameGuid} not found", gameGuid);
                return;
            }

            await SyncGameState(Context.ConnectionId, game);

            if (!isMoveValid)
            {
                _logger.LogWarning("Move was invalid for player {playerGuid} in game {gameGuid}", playerGuid, gameGuid);
                return;
            }

            if (BotService.CheckForConnectionExistance(Context.ConnectionId))
            {
                await MakeMoveBot(game, playerGuid, from, to);
                return;
            }

            var oponnent = game.GetOponnent(playerGuid);
            await SyncGameState(oponnent.ConnectionId, game);

            if (game.IsGameFinished())
            {
                _logger.LogInformation("Game {gameGuid} finished", gameGuid);
                await Clients.Client(oponnent.ConnectionId).SendAsync("EndOfGame", game.DidPlayerWin(oponnent.Guid));
                await Clients.Caller.SendAsync("EndOfGame", game.DidPlayerWin(playerGuid));
            }
        }

        public async Task StopGame(string? gameGuid, string playerGuid)
        {
            _logger.LogInformation("StopGame called with gameGuid: {gameGuid} and playerGuid: {playerGuid}", gameGuid, playerGuid);

            if (gameGuid == null)
            {
                _logger.LogInformation("Player {playerGuid} removed from queue", playerGuid);
                GameService.RemoveFromQueue(playerGuid);
                return;
            }

            GameService.StopGame(gameGuid);
            var game = GameService.GetGame(gameGuid);
            var oponnent = game.GetOponnent(playerGuid);
            await Clients.Client(oponnent.ConnectionId).SendAsync("GameStopped");
        }

        private async Task SyncGameState(string playerConnection, GameModel game)
        {
            _logger.LogInformation("SyncGameState called for player connection {playerConnection} in game {gameGuid}", playerConnection, game.Guid);

            await Clients.Client(playerConnection).SendAsync("SyncGameState", game.GetPlayerPieces(false), game.GetPlayerPieces(true), game.GetActivePlayerSymbol());
        }
    }
}
