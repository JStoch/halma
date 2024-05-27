namespace HalmaServer.Models {
    public class GameModel {
        public string GameGuid {get; set;}
        public PlayerModel Player1 {get; set;}
        public PlayerModel Player2 {get; set;}
        public List<PiecePositionModel> Pieces {get;}

        private bool Player1Turn {get; set;}
        private bool? DidPlayer1Win {get; set;}

        private List<(int, int)> P1Zone = [
            (0, 0),
            (0, 1),
            (0, 2),
            (0, 3),
            (0, 4),
            (1, 0),
            (1, 1),
            (1, 2),
            (1, 3),
            (1, 4),
            (2, 0),
            (2, 1),
            (2, 2),
            (2, 3),
            (3, 0),
            (3, 1),
            (3, 2),
            (4, 0),
            (4, 1)
        ];

        private List<(int, int)> P2Zone = [
            (15, 11),
            (15, 12),
            (15, 13),
            (15, 14),
            (15, 15),
            (14, 11),
            (14, 12),
            (14, 13),
            (14, 14),
            (14, 15),
            (13, 12),
            (13, 13),
            (13, 14),
            (13, 15),
            (12, 13),
            (12, 14),
            (12, 15),
            (11, 14),
            (11, 15),
        ];

        public GameModel(PlayerModel player1, PlayerModel player2) {
            GameGuid = Guid.NewGuid().ToString();
            Player1 = player1;
            Player2 = player2;
            Player1Turn = true;
            DidPlayer1Win = null;

            Pieces = [];
            P1Zone.ForEach( pieceCoord => Pieces.Add(new PiecePositionModel(pieceCoord.Item1, pieceCoord.Item2, this, player1)));
            P2Zone.ForEach( pieceCoord => Pieces.Add(new PiecePositionModel(pieceCoord.Item1, pieceCoord.Item2, this, player2)));
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
            var changedPiecesSet = GetPlayerPieces(Player1Turn);

            var hasPlayerWon = changedPiecesSet.All(piece => {
                var pieceCoord = (piece[0], piece[1]);
                if (Player1Turn) {
                    return P2Zone.Contains(pieceCoord);
                } else {
                    return P1Zone.Contains(pieceCoord);
                }
            });

            if (hasPlayerWon) {
                DidPlayer1Win = Player1Turn;
            }

            Player1Turn = !Player1Turn;
        }
    }
}