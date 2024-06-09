using BotHalma;
using BotHalma.Skynet;
using HalmaServer.Models;
using System.Text;

namespace backend.Services
{
    public enum DifficulityLevel
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2
    }

    public class BotDispacherService
    {
        private Dictionary<string, Bot> _botOponents;
        private Bot _currentBot;
        private Dictionary<string, Halma> _innerHalmas;
        private Halma _currentHalma;
        private DifficulityLevel _level;

        public BotDispacherService()
        {
            _botOponents = new Dictionary<string, Bot>(100);
            _innerHalmas = new Dictionary<string, Halma>(100);
        }

        public bool CheckForConnectionExistance(string connectionKey)
        {
            return this._innerHalmas.Keys.Contains(connectionKey) || this._botOponents.Keys.Contains(connectionKey);
        }

        public void InitForNewConnection(string connectionKey, DifficulityLevel level = DifficulityLevel.EASY, bool silenceFlag = false)
        {
            this._level = level;

            if (CheckForConnectionExistance(connectionKey)) return;

            try
            {
                MinMax minMaxForStrategies = new MinMax(1, 1, 1, false);
                    switch (_level)
                    {
                        case DifficulityLevel.EASY:

                            _currentBot = new AlphaBeta(2, depth: 3, width: 5, useAllStrategies: false);
                            _currentBot.SetStrategies
                                (
                                minMaxForStrategies.HDistanceToNearestFreeFieldInEnemyBase,
                                minMaxForStrategies.HNumberOfPawnsInMyBase,
                                minMaxForStrategies.HPrioritizeJumpMoves
                                );
                            break;

                        case DifficulityLevel.MEDIUM:

                            _currentBot = new AlphaBeta(2, depth: 6, width: 6, useAllStrategies: false);
                            _currentBot.SetStrategies
                                (
                                minMaxForStrategies.HDistanceToNearestFreeFieldInEnemyBase,
                                minMaxForStrategies.HNumberOfPawnsInMyBase,
                                minMaxForStrategies.HPrioritizeJumpMoves,
                                minMaxForStrategies.HAvoidBlockingMovesInBase,
                                minMaxForStrategies.HMaxTraverseDistance
                                );
                            break;

                        case DifficulityLevel.HARD:

                            _currentBot = new AlphaBeta(2, depth: 9, width: 7, useAllStrategies: true);
                            break;

                        default:
                            throw new ArgumentException($"Invalid difficulity level of {_level}");
                    }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (!silenceFlag)
                    throw new Exception($"Original exception thrown in {typeof(BotDispacherService)}: {ex}");
            }
            InitGameState(connectionKey);
        }
        private void InitGameState(string connectionKey)
        {

            Player player = new Player(1);
            _currentHalma = new Halma(new List<Player> { player, _currentBot});

            _innerHalmas.Add(connectionKey, _currentHalma);
            _botOponents.Add(connectionKey, _currentBot);

            //Info 
            // - depth <= 6 - <1s for move
            // - depth >= 10 - 1s for move

            _currentHalma.Init();

        }

        public void ClearConnectionIfExist(string connectionKey)
        {
            if (CheckForConnectionExistance(connectionKey))
            {
                if(_botOponents.Remove(connectionKey))
                    _currentBot = null;
                
                if(_innerHalmas.Remove(connectionKey));
                 _currentHalma.Dispose();
            }
        }

        internal static Halma.Move ConvertPiecesToMove(List<int> from, List<int> to)
        {
            return new Halma.Move(new Halma.PawnCoord(from[0], from[1]), new Halma.PawnCoord(to[0], to[1]));
        }

        internal static (List<int>, List<int>) ConvertMoveToPieces(Halma.Move move)
        {
            return (new List<int>() { move.From.X, move.From.Y }, new List<int>() { move.To.X, move.To.Y});
        }

        internal static (List<int>, List<int>) ReturnInvalidMove()
        {
            return ConvertMoveToPieces(Halma.Move.ReturnInvalidMove());
        }

        internal async Task<(List<int>, List<int>)> RequestOpponentMove(string connectionName, List<int> prevPlayerMoveFrom, List<int> prevPlayerMoveTo)
        {
            _innerHalmas.TryGetValue(connectionName, out _currentHalma);
            _botOponents.TryGetValue(connectionName, out _currentBot);

            var prevPlayerMove = ConvertPiecesToMove(prevPlayerMoveFrom, prevPlayerMoveTo);

            if (_currentHalma == null || _currentBot == null || !Halma.Move.IsValid(prevPlayerMove)) await Task.FromResult(ReturnInvalidMove());

            _currentHalma.MakeMove(prevPlayerMove, _currentBot.NextPlayer(_currentBot.Id));

            Halma.Move selectedMove = _currentBot.ChooseMove(_currentHalma.GameBoardState, ref Halma.Alphabet, out Halma.Move invalidMove);

            if (invalidMove != null || !Halma.Move.IsValid(selectedMove)) return await Task.FromResult(ReturnInvalidMove());


            _currentHalma.MakeMove(selectedMove, _currentBot.Id);

            return await Task.FromResult(ConvertMoveToPieces(selectedMove));

        }

        private static string ReadGameStateFromSTI(bool staticData = false)
        {
            string input = @"   | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 2 | 2 | 2 | 2 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 2 | 2 | 0 | 2 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 2 | 2 | 2 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 2 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 2 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 1 | 1 | 2 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 1 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
                                | 1 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |";

            string secondInput = @"0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2
                                    0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2
                                    0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0
                                    1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0
                                    1,1,1,1,1,0,0,0,2,0,0,0,0,0,0,0";

            if (staticData) return input;

            Console.WriteLine("Write board state:");
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;
            do
            {
                i = 0;
                while (i < Halma.BOARD_SIZE)
                {
                    string? line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine("Invalid input line. Try again!");
                        break;
                    }
                    else
                    {
                        stringBuilder.Append(line).Append(Environment.NewLine);
                    }
                    i++;
                }

            } while (i < Halma.BOARD_SIZE);

            //return stringBuilder.ToString();
            //return secondInput;
            return input;
        }
    }
}
