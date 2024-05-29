
namespace HalmaServer.Services {
    public class GameRepository() {

<<<<<<< Updated upstream
=======
        public void AddPlayer(PlayerModel player) {
            if (!Players.Contains(player)) {
                Players.Add(player);
            }
        }

        public PlayerModel? GetPlayerOrNull(string playerGuid) {
            return (from player in Players
                where player.PlayerGuid == playerGuid
                select player).FirstOrDefault();
        }

        public bool UpdatePlayerConnection(string playerGuid, string connectionId) {
            var playerToUpdate = (from player in Players
                where player.PlayerGuid == playerGuid
                select player).FirstOrDefault();
            
            if (playerToUpdate != null) {
                playerToUpdate.ConnectionId = connectionId;
                return true;
            } else {
                return false;
            }
        }

        public void AddGame(GameModel game) {
            if (!Games.Contains(game)) {
                Games.Add(game);
            }
        }

        public GameModel GetGame(string gameGuid) {
            return (from game in Games
                where game.GameGuid == gameGuid
                select game).First();
        }

        //TODO get from pieces not from game
        public bool UpdatePiecePosition(string pieceId, int x, int y, string gameGuid) {
            // delete from here
            var currentGame = (from game in Games
                where game.GameGuid == gameGuid
                select game).FirstOrDefault();

            if (currentGame == null) {
                return false;
            }

            // get piece from pieces list
            var pieceToUpdate = (from piece in currentGame.Pieces
                where piece.PieceId == pieceId
                select piece).FirstOrDefault();
            
            if (pieceToUpdate != null) {
                pieceToUpdate.X = x;
                pieceToUpdate.Y = y;
                return true;
            } else {
                return false;
            }
        }

        public bool UpdateIsGameActive(string gameGuid, bool isActive) {
            var currentGame = (from game in Games
                where game.GameGuid == gameGuid
                select game).FirstOrDefault();

            if (currentGame == null) {
                return false;
            }

            currentGame.IsGameActive = isActive;
            return true;
        }
>>>>>>> Stashed changes
    }
}