import Board from "../components/Board";
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { v4 as uuidv4 } from 'uuid';

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

const firstPlayer = 2;
const secondPlayer = 1;

function compareArrays(arr1, arr2) {
  return JSON.stringify(arr1) == JSON.stringify(arr2);
}

const ip = "localhost"
const port = 8080

function Game() {
  const [pieces, setPieces] = useState({ player1: [], player2: [] });
  const [player, setPlayer] = useState(0);
  const [turn, setTurn] = useState(0);
  const [dragging, setDragging] = useState(false);
  const [selectedPiece, setSelectedPiece] = useState(null);
  const [selectedField, setSelectedField] = useState(null);
  const [connection] = useState(new signalR.HubConnectionBuilder().withUrl(`http://${ip}:${port}/game`, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  }).build());
  const [uuid] = useState(uuidv4());
  const [gameuid, setGameUid] = useState(null);

  useEffect(() => {
    setPieces(staticPieces);

    connection.on("WaitingForGame", () => {
      //TODO react accordingly
    });

    connection.on("NewGame", (gameGuid, mySymbol) => {
      setGameUid(gameGuid);
      setPlayer(mySymbol);
    });

    connection.on("SyncGameState", (p1Pieces, p2Pieces, activePlayer) => {
      setPieces({
        player1: p1Pieces, 
        player2: p2Pieces
      });
      setTurn(activePlayer);
    });

    connection.on("EndOfGame", (didIWin) => {
      // TODO handle end of game (someone won)
      // WARNING this might be sent multiple times, but only once with "didIWin" param (not sure why)
      // let me know if it's a problem
    });

    connection.start().then(() => {
      connection.invoke("RequestNewGame", uuid);
    });
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
    if (!validMoves.find((piece) => compareArrays(piece, to)) 
      && (pieces["player1"].find((piece) => compareArrays(piece, middle)) ||
    pieces["player2"].find((piece) => compareArrays(piece, middle)))
      && validatePosition(from, to, turn)) 
    {
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
      addJumpIfValid(from, [currentPos[0] - 2, currentPos[1]], [currentPos[0] - 1, currentPos[1]], validMoves, jumpsQueue);
      addJumpIfValid(from, [currentPos[0] + 2, currentPos[1]], [currentPos[0] + 1, currentPos[1]], validMoves, jumpsQueue);
      addJumpIfValid(from, [currentPos[0], currentPos[1] - 2], [currentPos[0], currentPos[1] - 1], validMoves, jumpsQueue);
      addJumpIfValid(from, [currentPos[0], currentPos[1] + 2], [currentPos[0], currentPos[1] + 1], validMoves, jumpsQueue);

      // diagonal jumps
      addJumpIfValid(from, [currentPos[0] - 2, currentPos[1] - 2], [currentPos[0] - 1, currentPos[1] - 1], validMoves, jumpsQueue);
      addJumpIfValid(from, [currentPos[0] + 2, currentPos[1] + 2], [currentPos[0] + 1, currentPos[1] + 1], validMoves, jumpsQueue);
      addJumpIfValid(from, [currentPos[0] - 2, currentPos[1] + 2], [currentPos[0] - 1, currentPos[1] + 1], validMoves, jumpsQueue);
      addJumpIfValid(from, [currentPos[0] + 2, currentPos[1] - 2], [currentPos[0] + 1, currentPos[1] - 1], validMoves, jumpsQueue);
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
    </div>
  );
}

export default Game;
