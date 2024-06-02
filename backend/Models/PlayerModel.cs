namespace HalmaServer.Models {
    public class PlayerModel {
        public string PlayerGuid {get; set;}
        public string ConnectionId {get; set;}

        public PlayerModel(string playerGuid, string connectionId) {
            PlayerGuid = playerGuid;
            ConnectionId = connectionId;
        }

        public PlayerModel(string connectionId) {
            PlayerGuid = Guid.NewGuid().ToString();
            ConnectionId = connectionId;
        }
    }
}