import Board from "../components/Board";
import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

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

const staticPlayer = 2;
const staticTurn = 2;

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
  const [connection, setConnection] = useState(new signalR.HubConnectionBuilder().withUrl(`http://${ip}:${port}/game`, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  }).build());

  useEffect(() => {
    setPieces(staticPieces);
    setPlayer(staticPlayer);
    setTurn(staticTurn);

    connection.on("Answer", (param) => {
      console.log(`Got answer back: ${param}`);
    });

    connection.on("GameStopped", () => {
      // TODO this message means that the other player exited the game
      // the exiting player should use connection.invoke("StopGame", gameGuid, playerGuid)
    });

    connection.start().then(() => {
      connection.invoke("TestConnection", "Testest");
    });
  }, []);

  const validatePosition = (from, to, turn) => {
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
