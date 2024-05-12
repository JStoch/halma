import { useState } from "react";
import Field from "./Field";
import Piece from "./Piece";

Array.prototype.reverseIf = function (condition) {
  if (condition) {
    return this.reverse();
  }
  return this;
};

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
}) {
  const p1Pieces = pieces["player1"].map((piece) => piece[0] * 16 + piece[1]);
  const p2Pieces = pieces["player2"].map((piece) => piece[0] * 16 + piece[1]);
  const reverseBoard = player === 2;

  const handleMouseLeave = () => {
    setSelectedField(null);
  };

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
          >
            {p1Pieces.includes(i) ? (
              <Piece
                draggable={player === turn && player === 1}
                piecePlayer="1"
                setDragging={setDragging}
                setSelectedPiece={setSelectedPiece}
                piecePosition={[~~(i / 16), i % 16]}
                makeMove={makeMove}
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
              />
            ) : null}
          </Field>
        ))
        .reverseIf(reverseBoard)}
    </div>
  );
}

export default Board;
