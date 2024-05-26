using HalmaServer.Models;

namespace HalmaServer.Services {
    public class GameRepository() {
        //TODO connect to db
        //this whole part should be changed
        private List<PlayerModel> Players = [];
        private List<GameModel> Games = [];
        //end

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
    }
}
