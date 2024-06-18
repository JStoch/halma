using backend.Repositories;
using HalmaServer.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HalmaServer.Services
{
    public class GameService
    {
        private readonly GameRepository Repository;
        private readonly ILogger<GameService> Logger;

        // a list as queue, because a standard C# queue can't remove items from the middle
        private static List<PlayerModel> WaitingCustomGamePool = new();

        public GameService(GameRepository repository, ILogger<GameService> logger)
        {
            Repository = repository;
            Logger = logger;
        }

        public PlayerModel CreateBotPlayerModel()
        {
            Logger.LogInformation("Creating bot player model.");
            return new PlayerModel("BOT");
        }

        public GameModel? StartGameOrWait(string playerGuid, string connectionId, bool requestBotOponent = false)
        {
            Logger.LogInformation("Player {PlayerGuid} requested to start a game.", playerGuid);

            var player = GetPlayer(playerGuid, connectionId);
            if (WaitingCustomGamePool.Contains(player))
            {
                Logger.LogWarning("Player {PlayerGuid} is already in the waiting pool.", playerGuid);
                return null;
            }

            Repository.AddPlayer(player);
            Logger.LogInformation("Player {PlayerGuid} added to the repository.", playerGuid);

            if (WaitingCustomGamePool.Count == 0)
            {
                WaitingCustomGamePool.Add(player);
                Logger.LogInformation("Player {PlayerGuid} added to the waiting pool.", playerGuid);
                return null;
            }

            HalmaServer.Models.PlayerModel opponent;

            if (!requestBotOponent)
            {
                opponent = WaitingCustomGamePool[0];
                WaitingCustomGamePool.Remove(opponent);
                Logger.LogInformation("Player {PlayerGuid} matched with opponent {OpponentGuid}.", playerGuid, opponent.Guid);
            }
            else
            {
                opponent = CreateBotPlayerModel();
                Logger.LogInformation("Player {PlayerGuid} matched with bot opponent.", playerGuid);
            }

            var game = GameModel.GetGameModel(player, opponent);
            Repository.AddGame(game);
            Logger.LogInformation("Game created with ID {GameGuid} between player {PlayerGuid} and opponent {OpponentGuid}.", game.Guid, playerGuid, opponent.Guid);
            return game;
        }

        private PlayerModel GetPlayer(string playerGuid, string connectionId)
        {
            var player = Repository.GetPlayerOrNull(playerGuid);

            if (player != null)
            {
                player.ConnectionId = connectionId;
                Repository.UpdatePlayerConnection(playerGuid, connectionId);
                Logger.LogInformation("Updated connection ID for player {PlayerGuid}.", playerGuid);
            }
            else
            {
                player = new PlayerModel(playerGuid, connectionId);
                Logger.LogInformation("Created new player model for {PlayerGuid}.", playerGuid);
            }

            return player;
        }

        public bool MakeMove(string gameGuid, string playerGuid, List<int> from, List<int> to)
        {
            Logger.LogInformation("Player {PlayerGuid} is attempting to make a move in game {GameGuid} from ({FromX}, {FromY}) to ({ToX}, {ToY}).",
                playerGuid, gameGuid, from[0], from[1], to[0], to[1]);

            var game = Repository.GetGame(gameGuid);

            if (!game.CanPlayerMove(playerGuid) || from.Count != 2 || to.Count != 2)
            {
                Logger.LogWarning("Invalid move attempt by player {PlayerGuid} in game {GameGuid}.", playerGuid, gameGuid);
                return false;
            }

            var prevX = from[0];
            var prevY = from[1];
            var piece = game.Pieces.FirstOrDefault(movedPiece => movedPiece.X == prevX && movedPiece.Y == prevY);

            if (piece == null || piece.Owner.Guid != playerGuid)
            {
                Logger.LogWarning("Invalid piece or ownership for player {PlayerGuid} in game {GameGuid}.", playerGuid, gameGuid);
                return false;
            }

            Repository.UpdatePiecePosition(piece.Guid, to[0], to[1], game.Guid);
            game.NextMove();
            Repository.UpdateGameState(game);
            Logger.LogInformation("Move successful for player {PlayerGuid} in game {GameGuid}.", playerGuid, gameGuid);
            return true;
        }

        public GameModel? GetGame(string gameGuid)
        {
            Logger.LogInformation("Fetching game with ID {GameGuid}.", gameGuid);
            return Repository.GetGame(gameGuid);
        }

        public void StopGame(string gameGuid)
        {
            Logger.LogInformation("Stopping game with ID {GameGuid}.", gameGuid);
            var game = Repository.GetGame(gameGuid);
            game.IsGameActive = false;
            Repository.UpdateGameState(game);
        }

        public void RemoveFromQueue(string playerGuid)
        {
            Logger.LogInformation("Removing player {PlayerGuid} from the waiting pool.", playerGuid);
            WaitingCustomGamePool.Remove(GetPlayer(playerGuid, ""));
        }
    }
}
