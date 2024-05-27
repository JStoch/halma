
using HalmaServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace HalmaServer.Hubs
{
    public class GameHub(GameService gameService): Hub {

        private GameService GameService = gameService;
 
        public async Task RequestNewGame(string playerGuid)
        {
            var game = GameService.StartGameOrWait(playerGuid, Context.ConnectionId);
            if (game != null) {
                var player = game.GetPlayer(playerGuid);
                var oponnent = game.GetOponnent(playerGuid);
                await Clients.Client(oponnent.ConnectionId).SendAsync("NewGame", game.GameGuid, game.CanPlayerMove(oponnent.PlayerGuid));
                await Clients.Caller.SendAsync("NewGame", game.GameGuid, game.CanPlayerMove(player.PlayerGuid));
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
            await Clients.Caller.SendAsync("SyncGameState", game.GetPlayerPieces(true), game.GetPlayerPieces(false), game.CanPlayerMove(playerGuid));

            if (!isMoveValid) {
                return;
            }
            
            var oponnent = game.GetOponnent(playerGuid);
            await Clients.Client(oponnent.ConnectionId).SendAsync("SyncGameState", game.GetPlayerPieces(true), game.GetPlayerPieces(false), game.CanPlayerMove(oponnent.PlayerGuid));
            
            if (game.IsGameFinished()) {
                await Clients.Client(oponnent.ConnectionId).SendAsync("EndOfGame", game.DidPlayerWin(oponnent.PlayerGuid));
                await Clients.Caller.SendAsync("EndOfGame", game.DidPlayerWin(playerGuid));
            }
        }

    }
}