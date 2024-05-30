import Board from "../components/Board";
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { v4 as uuidv4 } from "uuid";
import Button from "../components/Button";

const staticPieces = {
  player1: [
    [11, 14],
    [11, 15],
    [12, 13],
    [12, 14],
    [12, 15],
    [13, 12],
    [13, 13],
    [13, 14],
    [13, 15],
    [14, 11],
    [14, 12],
    [14, 13],
    [14, 14],
    [14, 15],
    [15, 11],
    [15, 12],
    [15, 13],
    [15, 14],
    [15, 15],
  ],
  player2: [
    [0, 0],
    [0, 1],
    [0, 2],
    [0, 3],
    [0, 4],
    [1, 0],
    [1, 1],
    [1, 2],
    [1, 3],
    [1, 4],
    [2, 0],
    [2, 1],
    [2, 2],
    [2, 3],
    [3, 0],
    [3, 1],
    [3, 2],
    [4, 0],
    [4, 1],
  ],
};

const loadingTextArray = [
  "Strategizing Moves...",
  "Setting Up the Board...",
  "Assembling Pieces...",
  "Loading Your Halma Experience...",
  "Getting Your Opponents Ready...",
];

const firstPlayer = 2;
const secondPlayer = 1;

function compareArrays(arr1, arr2) {
  return JSON.stringify(arr1) == JSON.stringify(arr2);
}

const ip = "localhost";
const port = 8080;

function Game() {
  const [pieces, setPieces] = useState({ player1: [], player2: [] });
  const [player, setPlayer] = useState(0);
  const [turn, setTurn] = useState(0);
  const [dragging, setDragging] = useState(false);
  const [selectedPiece, setSelectedPiece] = useState(null);
  const [selectedField, setSelectedField] = useState(null);
  const [connection] = useState(
    new signalR.HubConnectionBuilder()
      .withUrl(`http://${ip}:${port}/game`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build()
  );
  const [uuid] = useState(uuidv4());
  const [gameuid, setGameUid] = useState(null);
  const [loading, setLoading] = useState(true);
  const [loadingText, setLoadingText] = useState(loadingTextArray[0]);

  useEffect(() => {
    setPieces(staticPieces);

    connection.on("WaitingForGame", () => {
      //TODO react accordingly
    });

    connection.on("NewGame", (gameGuid, mySymbol) => {
      console.log("asd");
      setLoading(false);
      setGameUid(gameGuid);
      setPlayer(mySymbol);
    });

    connection.on("SyncGameState", (p1Pieces, p2Pieces, activePlayer) => {
      setPieces({
        player1: p1Pieces,
        player2: p2Pieces,
      });
      setTurn(activePlayer);
    });

    connection.on("EndOfGame", (didIWin) => {
      // TODO handle end of game (someone won)
      // WARNING this might be sent multiple times, but only once with "didIWin" param (not sure why)
      // let me know if it's a problem
    });

    connection.on("GameStopped", () => {
      // TODO this message means that the other player exited the game
      // the exiting player should use connection.invoke("StopGame", gameGuid, playerGuid)
    });

    connection.start().then(() => {
      setLoading(true);
      connection.invoke("RequestNewGame", uuid);
    });

    const interval = setInterval(() => {
      setLoadingText((prevText) => {
        const currentIndex = loadingTextArray.indexOf(prevText);
        const nextIndex = (currentIndex + 1) % loadingTextArray.length;
        return loadingTextArray[nextIndex];
      });
    }, 5000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  const validatePosition = (from, to, turn) => {
    // check if target is on board
    if (to[0] < 0 || to[1] < 0 || to[0] > 15 || to[1] > 15) {
      return false;
    }

    const p1Pieces = pieces["player1"];
    const p2Pieces = pieces["player2"];

    // check if target field is not occupied
    if (
      p1Pieces.find((piece) => compareArrays(piece, to)) ||
      p2Pieces.find((piece) => compareArrays(piece, to))
    ) {
      return false;
    }

    // validate for player 1 turn
    if (turn === 1) {
      // check if piece belongs to player 1
      if (!p1Pieces.find((piece) => compareArrays(piece, from))) {
        return false;
      }
      return true;
    }
    // validate for player 2 turn
    if (turn === 2) {
      // check if piece belongs to player 2
      if (!p2Pieces.find((piece) => compareArrays(piece, from))) {
        return false;
      }
      return true;
    }

    return false;
  };

  const addMoveIfValid = (from, to, validMoves) => {
    if (validatePosition(from, to, turn)) {
      validMoves.push(to);
    }
  };

  const addJumpIfValid = (from, to, middle, validMoves, jumpQueue) => {
    // add only if this position hasn't already been visited
    // and the middle is occupied
    if (
      !validMoves.find((piece) => compareArrays(piece, to)) &&
      (pieces["player1"].find((piece) => compareArrays(piece, middle)) ||
        pieces["player2"].find((piece) => compareArrays(piece, middle))) &&
      validatePosition(from, to, turn)
    ) {
      validMoves.push(to);
      jumpQueue.push(to);
    }
  };

  const getValidMoves = (from) => {
    var validMoves = [];

    // side moves
    addMoveIfValid(from, [from[0] + 1, from[1]], validMoves);
    addMoveIfValid(from, [from[0] - 1, from[1]], validMoves);
    addMoveIfValid(from, [from[0], from[1] + 1], validMoves);
    addMoveIfValid(from, [from[0], from[1] - 1], validMoves);

    // diagonal moves
    addMoveIfValid(from, [from[0] + 1, from[1] + 1], validMoves);
    addMoveIfValid(from, [from[0] - 1, from[1] + 1], validMoves);
    addMoveIfValid(from, [from[0] + 1, from[1] - 1], validMoves);
    addMoveIfValid(from, [from[0] - 1, from[1] - 1], validMoves);

    // jumps
    var jumpsQueue = [from];
    while (jumpsQueue.length > 0) {
      var currentPos = jumpsQueue.shift();

      // side jumps
      addJumpIfValid(
        from,
        [currentPos[0] - 2, currentPos[1]],
        [currentPos[0] - 1, currentPos[1]],
        validMoves,
        jumpsQueue
      );
      addJumpIfValid(
        from,
        [currentPos[0] + 2, currentPos[1]],
        [currentPos[0] + 1, currentPos[1]],
        validMoves,
        jumpsQueue
      );
      addJumpIfValid(
        from,
        [currentPos[0], currentPos[1] - 2],
        [currentPos[0], currentPos[1] - 1],
        validMoves,
        jumpsQueue
      );
      addJumpIfValid(
        from,
        [currentPos[0], currentPos[1] + 2],
        [currentPos[0], currentPos[1] + 1],
        validMoves,
        jumpsQueue
      );

      // diagonal jumps
      addJumpIfValid(
        from,
        [currentPos[0] - 2, currentPos[1] - 2],
        [currentPos[0] - 1, currentPos[1] - 1],
        validMoves,
        jumpsQueue
      );
      addJumpIfValid(
        from,
        [currentPos[0] + 2, currentPos[1] + 2],
        [currentPos[0] + 1, currentPos[1] + 1],
        validMoves,
        jumpsQueue
      );
      addJumpIfValid(
        from,
        [currentPos[0] - 2, currentPos[1] + 2],
        [currentPos[0] - 1, currentPos[1] + 1],
        validMoves,
        jumpsQueue
      );
      addJumpIfValid(
        from,
        [currentPos[0] + 2, currentPos[1] - 2],
        [currentPos[0] + 1, currentPos[1] - 1],
        validMoves,
        jumpsQueue
      );
    }
    return validMoves;
  };

  const makeMove = () => {
    if (!dragging || selectedField == null || selectedPiece == null) {
      return;
    }

    const playerPieces = turn === 1 ? pieces["player1"] : pieces["player2"];

    if (!validatePosition(selectedPiece, selectedField, turn)) {
      setSelectedField(null);
      setSelectedPiece(null);
      return;
    }

    if (turn === 1) {
      setPieces({
        ...pieces,
        player1: playerPieces.map((piece) =>
          compareArrays(piece, selectedPiece) ? selectedField : piece
        ),
      });
    }
    if (turn === 2) {
      setPieces({
        ...pieces,
        player2: playerPieces.map((piece) =>
          compareArrays(piece, selectedPiece) ? selectedField : piece
        ),
      });
    }

    connection.invoke("MakeMove", gameuid, uuid, selectedPiece, selectedField);
    setSelectedField(null);
    setSelectedPiece(null);
  };

  return (
    <div className="flex-1 flex justify-center items-center">
      {loading ? (
        <div className="flex flex-col items-center gap-2">
          <svg
            aria-hidden="true"
            className="inline w-12 h-12 text-platinum animate-spin dark:text-gray-600 fill-gray-600 dark:fill-gray-300"
            viewBox="0 0 100 101"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
              fill="currentColor"
            />
            <path
              d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
              fill="currentFill"
            />
          </svg>
          <span className="text-platinum text-2xl font-semibold">
            {loadingText}
          </span>
          <Button
            value="Cancel"
            className="bg-red-500 hover:bg-red-400 relative top-6"
          />
        </div>
      ) : (
        <Board
          className="h-6/7"
          pieces={pieces}
          player={player}
          turn={turn}
          dragging={dragging}
          setDragging={setDragging}
          setSelectedPiece={setSelectedPiece}
          setSelectedField={setSelectedField}
          makeMove={makeMove}
        />
      )}
    </div>
  );
}

export default Game;
