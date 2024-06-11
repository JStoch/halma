import { useState } from "react";
import Field from "./Field";
import Piece from "./Piece";

Array.prototype.reverseIf = function (condition) {
  if (condition) {
    return this.reverse();
  }
  return this;
};

function compareArrays(arr1, arr2) {
  return JSON.stringify(arr1) == JSON.stringify(arr2);
}

function Board({
  player,
  turn,
  className,
  dragging,
  setDragging,
  setSelectedField,
  setSelectedPiece,
  makeMove,
  pieces = { player1: [], player2: [] },
  getValidMoves,
}) {
  const [highlightedPiece, setHighlightedPiece] = useState(null);
  const [shouldQuitHighlighting, setShouldQuitHighlighting] = useState(false);
  const p1Pieces = pieces["player1"].map((piece) => piece[0] * 16 + piece[1]);
  const p2Pieces = pieces["player2"].map((piece) => piece[0] * 16 + piece[1]);
  const reverseBoard = player === 2;

  const handleMouseLeave = () => {
    if (setSelectedField) {
      setSelectedField(null);
    }
  };

  const handleSetHighlightedPiece = (newPiece) => {
    setShouldQuitHighlighting(false);
    if (!compareArrays(newPiece, highlightedPiece)) {
      setHighlightedPiece(newPiece);
    } else {
      setShouldQuitHighlighting(true);
    }
  };

  const highlightedPieceCode = highlightedPiece
    ? highlightedPiece[0] * 16 + highlightedPiece[1]
    : null;

  let validMoves = highlightedPiece ? getValidMoves(highlightedPiece) : [];
  if (validMoves) {
    validMoves = validMoves.map((pos) => pos[0] * 16 + pos[1]);
  }
  console.log(validMoves);

  return (
    <div
      className={`${className} aspect-square bg-slate-500 flex flex-wrap`}
      onMouseLeave={handleMouseLeave}
    >
      {[...Array(16 * 16)]
        .map((e, i) => (
          <Field
            key={i}
            dragging={dragging}
            color={(i + ~~(i / 16)) % 2 ? "white" : "black"}
            setSelectedField={setSelectedField}
            fieldPosition={[~~(i / 16), i % 16]}
            highlighted={i === highlightedPieceCode}
            movePossible={validMoves.includes(i)}
          >
            {p1Pieces.includes(i) ? (
              <Piece
                draggable={player === turn && player === 1}
                piecePlayer="1"
                setDragging={setDragging}
                setSelectedPiece={setSelectedPiece}
                piecePosition={[~~(i / 16), i % 16]}
                makeMove={makeMove}
                setHighlightedPiece={handleSetHighlightedPiece}
                highlighted={i === highlightedPieceCode}
                shouldQuitHighlighting={shouldQuitHighlighting}
              />
            ) : null}
            {p2Pieces.includes(i) ? (
              <Piece
                draggable={player === turn && player === 2}
                piecePlayer="2"
                setDragging={setDragging}
                setSelectedPiece={setSelectedPiece}
                piecePosition={[~~(i / 16), i % 16]}
                makeMove={makeMove}
                setHighlightedPiece={handleSetHighlightedPiece}
                highlighted={i === highlightedPieceCode}
                shouldQuitHighlighting={shouldQuitHighlighting}
              />
            ) : null}
          </Field>
        ))
        .reverseIf(reverseBoard)}
    </div>
  );
}

export default Board;
