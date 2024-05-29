
using HalmaServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace HalmaServer.Hubs
{
    public class GameHub(GameService gameService): Hub {

        private GameService GameService = gameService;
<<<<<<< Updated upstream
        //TODO remove
        public async Task TestConnection(string param) {
            Console.WriteLine("Message recieved: " + param);
            await Clients.Caller.SendAsync("Answer", Context.ConnectionId);
=======
 
        public async Task RequestNewGame(string playerGuid)
        {
            var game = GameService.StartGameOrWait(playerGuid, Context.ConnectionId);
            if (game != null) {
                var player = game.GetPlayer(playerGuid);
                var oponnent = game.GetOponnent(playerGuid);
                // start game
                await Clients.Client(oponnent.ConnectionId).SendAsync("NewGame", game.GameGuid, game.GetPlayerSymbol(oponnent.PlayerGuid));
                await Clients.Caller.SendAsync("NewGame", game.GameGuid, game.GetPlayerSymbol(player.PlayerGuid));

                // sync state for the start
                await SyncGameState(Context.ConnectionId, game);
                await SyncGameState(oponnent.ConnectionId, game);
            } else {
                await Clients.Caller.SendAsync("WaitingForGame");
            }
        }

        public async Task MakeMove(string gameGuid, string playerGuid, List<int> from, List<int> to) {
            var isMoveValid = GameService.MakeMove(gameGuid, playerGuid, from, to);
            var game = GameService.GetGame(gameGuid);
            if (game == null) {
                return;
            }

            // always sync the caller, send data to the oponnent only if the move was valid
            await SyncGameState(Context.ConnectionId, game);
            
            if (!isMoveValid) {
                return;
            }
            
            var oponnent = game.GetOponnent(playerGuid);
            await SyncGameState(oponnent.ConnectionId, game);

            if (game.IsGameFinished()) {
                await Clients.Client(oponnent.ConnectionId).SendAsync("EndOfGame", game.DidPlayerWin(oponnent.PlayerGuid));
                await Clients.Caller.SendAsync("EndOfGame", game.DidPlayerWin(playerGuid));
            }
        }

        public async Task StopGame(string gameGuid, string playerGuid) {
            GameService.StopGame(gameGuid);
            var game = GameService.GetGame(gameGuid);
            var oponnent = game.GetOponnent(playerGuid);
            await Clients.Client(oponnent.ConnectionId).SendAsync("GameStopped");
        }
        
        private async Task SyncGameState(string playerConnection, GameModel game) {
            await Clients.Client(playerConnection).SendAsync("SyncGameState", game.GetPlayerPieces(false), game.GetPlayerPieces(true), game.GetActivePlayerSymbol());
>>>>>>> Stashed changes
        }

    }
}