using HalmaServer.Models;

namespace HalmaServer.Services {
    public class GameService(GameRepository repository) {
        private GameRepository Repository = repository;
        private Queue<PlayerModel> WaitingCustomGamePool = new();

        public GameModel? StartGameOrWait(string playerGuid, string connectionId) {
            var player = GetPlayer(playerGuid, connectionId);
            if (WaitingCustomGamePool.Contains(player)) {
                return null;
            }

            Repository.AddPlayer(player);

            if (WaitingCustomGamePool.Count == 0) {
                WaitingCustomGamePool.Enqueue(player);
                return null;
            }

            var oponnent = WaitingCustomGamePool.Dequeue();

            var game = new GameModel(player, oponnent);
            Repository.AddGame(game);
            return game;
        } 

        private PlayerModel GetPlayer(string playerGuid, string connectionId) {
            var player = Repository.GetPlayerOrNull(playerGuid);
            
            if (player != null) {
                player.ConnectionId = connectionId;
                Repository.UpdatePlayerConnection(playerGuid, connectionId);
            } else {
                player = new PlayerModel(playerGuid, connectionId);
            }
            return player;
        }

        public bool MakeMove(string gameGuid, string playerGuid, List<int> from, List<int> to) {
            var game = Repository.GetGame(gameGuid);

            if (!game.CanPlayerMove(playerGuid) || from.Count != 2 || to.Count != 2) {
                return false;
            }

            var prevX = from[0];
            var prevY = from[1];
            var piece = (from movedPiece in game.Pieces
                where movedPiece.X == prevX && movedPiece.Y == prevY
                select movedPiece).FirstOrDefault();

            if (piece == null || piece.Owner.PlayerGuid != playerGuid) {
                return false;
            }

            Repository.UpdatePiecePosition(piece.PieceId, to[0], to[1], game.GameGuid);
            game.NextMove();
            return true;
        }

        public GameModel GetGame(string gameGuid) {
            return Repository.GetGame(gameGuid);
        }

        public void StopGame(string gameGuid) {
            Repository.UpdateIsGameActive(gameGuid, false);
        }

    }
}