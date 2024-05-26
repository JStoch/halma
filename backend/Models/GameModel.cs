namespace HalmaServer.Models {
    public class GameModel {
        public string GameGuid {get; set;}
        public PlayerModel Player1 {get; set;}
        public PlayerModel Player2 {get; set;}
        private bool Player1Turn {get; set;}

        public GameModel(PlayerModel player1, PlayerModel player2) {
            GameGuid = Guid.NewGuid().ToString();
            Player1 = player1;
            Player2 = player2;
            Player1Turn = true;
        }

        public PlayerModel GetPlayer(string playerGuid) {
            if (Player1.PlayerGuid == playerGuid) {
                return Player1;
            } else {
                return Player2;
            }
        }

        public PlayerModel GetOponnent(string playerGuid) {
            if (Player1.PlayerGuid != playerGuid) {
                return Player1;
            } else {
                return Player2;
            }
        }

        public bool CanPlayerMove(string playerGuid) {
            return Player1.PlayerGuid == playerGuid && Player1Turn;
        }
    }
}