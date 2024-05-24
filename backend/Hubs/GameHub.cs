
using HalmaServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace HalmaServer.Hubs
{
    public class GameHub(GameService gameService): Hub {

        private GameService GameService = gameService;
        //TODO remove
        public async Task TestConnection(string param) {
            Console.WriteLine("Message recieved: " + param);
            await Clients.Caller.SendAsync("Answer", Context.ConnectionId);
        }

    }
}