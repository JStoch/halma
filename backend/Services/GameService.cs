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

    }
}