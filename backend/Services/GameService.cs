namespace HalmaServer.Services {
    public class GameService(GameRepository repository) {
        private GameRepository Repository = repository;

        public void StopGame(string gameGuid) {
            Repository.UpdateIsGameActive(gameGuid, false);
        }

    }
}