
using backend.Services;
using HalmaServer.Models;
using HalmaServer.Services;
using Microsoft.AspNetCore.SignalR;
using Mono.TextTemplating;

namespace HalmaServer.Hubs
{
    public class GameHub: Hub {

        private GameService GameService { get; set; }
        private BotDispacherService BotService { get; set; }

        public GameHub(GameService gameService, BotDispacherService botDispacherService) : base()
        {
            this.GameService = gameService;
            this.BotService = botDispacherService;
        }
 
        public async Task RequestNewGame(string playerGuid)
        {
            var game = GameService.StartGameOrWait(playerGuid, Context.ConnectionId);
            if (game != null) {
                var player = game.GetPlayer(playerGuid);
                var oponnent = game.GetOponnent(playerGuid);
                // start game
                await Clients.Client(oponnent.ConnectionId).SendAsync("NewGame", game.GameGuid, game.GetPlayerSymbol(oponnent.PlayerGuid));
                await Clients.Caller.SendAsync("NewGame", game.GameGuid, game.GetPlayerSymbol(player.PlayerGuid));

                // sync state for the start
                await SyncGameState(Context.ConnectionId, game);
                await SyncGameState(oponnent.ConnectionId, game);
            } else {
                await Clients.Caller.SendAsync("WaitingForGame");
            }
        }

        public async Task RequestNewGameWithBot(string playerGuid, DifficulityLevel difficulityLevel = DifficulityLevel.EASY)
        {
            var game = GameService.StartGameOrWait(playerGuid, Context.ConnectionId, requestBotOponent: true);
            if (game != null)
            {
                var player = game.GetPlayer(playerGuid);
                // start game
                await Clients.Caller.SendAsync("NewGameWithBot", game.GameGuid, game.GetPlayerSymbol(player.PlayerGuid));

                // sync state for the start
                await SyncGameState(Context.ConnectionId, game);
                BotService.InitForNewConnection(Context.ConnectionId, level: difficulityLevel);
            }
            else
            {
                await Clients.Caller.SendAsync("WaitingForGameWithBot");
            }
        }

        private async Task MakeMoveBot(GameModel game, string playerGuid, List<int> from, List<int> to)
        {
            (List<int> botFrom, List<int> botTo) = await BotService.RequestOpponentMove(Context.ConnectionId, from, to);
            var isMoveValid = GameService.MakeMove(game.GameGuid, game.Player2Guid, botFrom, botTo);

            if (!isMoveValid) return;

            await SyncGameState(Context.ConnectionId, game);

            if (game.IsGameFinished())
            {
                await Clients.Caller.SendAsync("EndOfGame", game.DidPlayerWin(playerGuid));
               BotService.ClearConnectionIfExist(Context.ConnectionId);
            }

        }

        public async Task MakeMove(string gameGuid, string playerGuid, List<int> from, List<int> to) {
            var isMoveValid = GameService.MakeMove(gameGuid, playerGuid, from, to);
            var game = GameService.GetGame(gameGuid);
            if (game == null) {
                return;
            }

            

            // always sync the caller, send data to the oponnent only if the move was valid
            await SyncGameState(Context.ConnectionId, game);
            
            if (!isMoveValid) {
                return;
            }


            // if bot service is configured for this connection, opponent move is performed by bot
            if (BotService.CheckForConnectionExistance(Context.ConnectionId))
            {
                await MakeMoveBot(game, playerGuid, from, to);
                return;
            }

            var oponnent = game.GetOponnent(playerGuid);
            await SyncGameState(oponnent.ConnectionId, game);

            if (game.IsGameFinished()) {
                await Clients.Client(oponnent.ConnectionId).SendAsync("EndOfGame", game.DidPlayerWin(oponnent.PlayerGuid));
                await Clients.Caller.SendAsync("EndOfGame", game.DidPlayerWin(playerGuid));
            }
        }

        public async Task StopGame(string? gameGuid, string playerGuid) {
            if (gameGuid == null) {
                GameService.RemoveFromQueue(playerGuid);
                return;
            }

            GameService.StopGame(gameGuid);
            var game = GameService.GetGame(gameGuid);
            var oponnent = game.GetOponnent(playerGuid);
            await Clients.Client(oponnent.ConnectionId).SendAsync("GameStopped");
        }
        
        private async Task SyncGameState(string playerConnection, GameModel game) {
            await Clients.Client(playerConnection).SendAsync("SyncGameState", game.GetPlayerPieces(false), game.GetPlayerPieces(true), game.GetActivePlayerSymbol());
        }

    }
}