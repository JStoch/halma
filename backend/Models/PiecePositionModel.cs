namespace HalmaServer.Models {
    public class PiecePositionModel(int x, int y, GameModel game, PlayerModel owner)
    {
        //TODO change for a generated key
        public string PieceId {get; set;} = Guid.NewGuid().ToString();
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public GameModel Game { get; } = game;
        public PlayerModel Owner { get; } = owner;
    }
}