using HalmaServer.Models;
using HalmaWebApi.DbContexts;
using HalmaWebApi.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    //TODO save changes in each function to db
    public class GameRepository
    {
        //TODO connect to db
        //this whole part should be changed

        private AsyncRepository<GameHistory, HalmaDbContext> GamesHistory
        {
            get
            {
                return _repositoryFactory.GetRepo<GameHistory>();
            }
        }
        private AsyncRepository<PlayerModel, HalmaDbContext> Players
        {
            get
            {
                return _repositoryFactory.GetRepo<PlayerModel>();
            }
        }

        private AsyncRepository<PiecePositionModel, HalmaDbContext> Pieces
        {
            get
            {
                return _repositoryFactory.GetRepo<PiecePositionModel>();
            }
        }


        private AsyncRepository<GameModel, HalmaDbContext> Games
        {
            get
            {
                return _repositoryFactory.GetRepo<GameModel>();
            }
        }

        private AsyncRepository<Statistic, HalmaDbContext> Statistics
        {
            get
            {
                return _repositoryFactory.GetRepo<Statistic>();
            }
        }
        //TODO add a list of pieces
        //end
        private RepositoryFactory<HalmaDbContext> _repositoryFactory;

        public GameRepository(RepositoryFactory<HalmaDbContext> repo)
        {
            _repositoryFactory = repo;
        }

        public void AddPlayer(PlayerModel player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }

        public PlayerModel? GetPlayerOrNull(string playerGuid)
        {
            return Players.Get(playerGuid).Result;
        }

        public bool UpdatePlayerConnection(string playerGuid, string connectionId)
        {
            var playerToUpdate = Players.Get(playerGuid).Result;

            if (playerToUpdate != null)
            {
                playerToUpdate.ConnectionId = connectionId;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddGame(GameModel game)
        {
            if (!Games.Contains(game))
            {
                Games.Add(game);
                
            }
            
        }

        public GameModel? GetGame(string gameGuid)
        {
            return Games.Get(gameGuid).Result;
        }

        //TODO get from pieces not from game
        public bool UpdatePiecePosition(string pieceId, int x, int y, string gameGuid)
        {
            // delete from here
            var currentGame = Games.Get(pieceId).Result;    

            if (currentGame == null)
            {
                return false;
            }

            // get piece from pieces list
            var pieceToUpdate = Pieces.Get(pieceId).Result;

            if (pieceToUpdate != null)
            {
                pieceToUpdate.X = x;
                pieceToUpdate.Y = y;
                return true;
            }
            else
            {
                return false;
            }
        }

        // This function is called after changing: GameModel.Player1Turn
        // And possibly GameModel.DidPlayer1Win, GameModel.IsGameActive
        // [KW] Yup lets roll with this shit!
        public bool UpdateGameState(GameModel game)
        {
            if (!Games.Contains(game)) return false;

            Games.Update(game);

            //If game has ended - update statistics
            if (!game.IsGameActive)
            {
                Statistic player1Stats = Statistics.FindAsyncRefPlayer(s => s.Equals(game.Player1Guid)).Result ?? new Statistic() { PlayerGuid = game.Player1Guid }; 
                Statistic player2Stats = Statistics.FindAsyncRefPlayer(s => s.Equals(game.Player2Guid)).Result ?? new Statistic() { PlayerGuid = game.Player2Guid };



                if (game.DidPlayerWin(game.Player1.PlayerGuid))
                {
                    player1Stats.UpdateWin();
                    player2Stats.UpdateLoss();
                }
                else
                {
                    player1Stats.UpdateLoss();
                    player2Stats.UpdateWin();
                }

                if (!Statistics.Contains(player1Stats)) // check for ids
                    Statistics.Add(player1Stats);
                else
                    Statistics.Update(player1Stats);

                if (!Statistics.Contains(player2Stats))
                    Statistics.Add(player2Stats);
                else
                    Statistics.Update(player2Stats);
                
                if(GamesHistory.Any(gh => gh.GameModel != null && gh.GameModel.Equals(game)).Result)
                {
                    var gameHistory = new GameHistory()
                    {
                        GameModel = game
                    };

                    GamesHistory.Add(gameHistory);
                }

            }

            _ = _repositoryFactory.SaveChangesAsync(new HashSet<Type>() {typeof(Statistic), typeof(GameHistory)});

            return true;
        }
    }
}
