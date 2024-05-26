
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

    }
}