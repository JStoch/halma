namespace HalmaServer.Models {
    public class GameModel {
        public string GameGuid {get; set;}
        public PlayerModel Player1 {get; set;}
        public PlayerModel Player2 {get; set;}
        public List<PiecePositionModel> Pieces {get;}

        private bool Player1Turn {get; set;}
        private bool? DidPlayer1Win {get; set;}

        public GameModel(PlayerModel player1, PlayerModel player2) {
            GameGuid = Guid.NewGuid().ToString();
            Player1 = player1;
            Player2 = player2;
            Player1Turn = true;
            DidPlayer1Win = false;

            Pieces  = [
                new PiecePositionModel(0, 0, this, Player1),
                new PiecePositionModel(0, 1, this, Player1),
                new PiecePositionModel(0, 2, this, Player1),
                new PiecePositionModel(0, 3, this, Player1),
                new PiecePositionModel(0, 4, this, Player1),
                new PiecePositionModel(1, 0, this, Player1),
                new PiecePositionModel(1, 1, this, Player1),
                new PiecePositionModel(1, 2, this, Player1),
                new PiecePositionModel(1, 3, this, Player1),
                new PiecePositionModel(1, 4, this, Player1),
                new PiecePositionModel(2, 0, this, Player1),
                new PiecePositionModel(2, 1, this, Player1),
                new PiecePositionModel(2, 2, this, Player1),
                new PiecePositionModel(2, 3, this, Player1),
                new PiecePositionModel(3, 0, this, Player1),
                new PiecePositionModel(3, 1, this, Player1),
                new PiecePositionModel(3, 2, this, Player1),
                new PiecePositionModel(4, 0, this, Player1),
                new PiecePositionModel(4, 1, this, Player1),
                new PiecePositionModel(15, 11, this, Player2),
                new PiecePositionModel(15, 12, this, Player2),
                new PiecePositionModel(15, 13, this, Player2),
                new PiecePositionModel(15, 14, this, Player2),
                new PiecePositionModel(15, 15, this, Player2),
                new PiecePositionModel(14, 11, this, Player2),
                new PiecePositionModel(14, 12, this, Player2),
                new PiecePositionModel(14, 13, this, Player2),
                new PiecePositionModel(14, 14, this, Player2),
                new PiecePositionModel(14, 15, this, Player2),
                new PiecePositionModel(13, 12, this, Player2),
                new PiecePositionModel(13, 13, this, Player2),
                new PiecePositionModel(13, 14, this, Player2),
                new PiecePositionModel(13, 15, this, Player2),
                new PiecePositionModel(12, 13, this, Player2),
                new PiecePositionModel(12, 14, this, Player2),
                new PiecePositionModel(12, 15, this, Player2),
                new PiecePositionModel(11, 14, this, Player2),
                new PiecePositionModel(11, 15, this, Player2)
            ];
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
            return Player1.PlayerGuid == playerGuid == Player1Turn;
        }

        public bool IsGameFinished() {
            return DidPlayer1Win != null;
        }

        public bool DidPlayerWin(string playerGuid) {
            return Player1.PlayerGuid == playerGuid == DidPlayer1Win;
        }

        public List<List<int>> GetPlayerPieces(bool forPlayer1) {
            var playerGuid = forPlayer1 ? Player1.PlayerGuid : Player2.PlayerGuid;
            return (from piece in Pieces
                where piece.Owner.PlayerGuid == playerGuid
                select new List<int>{ piece.X, piece.Y }).ToList();
        }

        public void NextMove() {
            Player1Turn = !Player1Turn;
        }
    }
}