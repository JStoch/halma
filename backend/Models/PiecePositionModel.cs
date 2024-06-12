using backend.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalmaServer.Models {
    public class PiecePositionModel : IGetGuid
    {
        private PiecePositionModel(int x, int y)
        {
            Guid = System.Guid.NewGuid().ToString();
            X = x;
            Y = y;
        }

        public static PiecePositionModel GetNewPiecePositionModel(int x , int y, GameModel game, PlayerModel owner)
        {
            var piecePosition = new PiecePositionModel(x,y)
            {
                Game = game,
                Owner = owner
            };

            return piecePosition;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Guid {get;  set;}
        public int X { get; set; }
        public int Y { get; set; }

        [ForeignKey("GameGuid")]
        public GameModel Game { get; private set; }
        
        [ForeignKey("PlayerGuid")]
        public string OwnerGuid { get; set; }
        public PlayerModel Owner { get; private set; }

        public string GetGuid()
        {
            return Guid;
        }
    }
}