import Draggable from "react-draggable";
import { useRef, useState } from "react";

function Piece({
  draggable,
  piecePlayer,
  setDragging,
  piecePosition,
  setSelectedPiece,
  makeMove,
}) {
  const pieceRef = useRef(null);
  const [pieceDragging, setPieceDragging] = useState(false);
  const pos = { x: 0, y: 0 };
  const bgColor = piecePlayer === "1" ? "bg-blue-500" : "bg-red-500";
  const borderColor =
    piecePlayer === "1" ? "border-blue-700" : "border-red-700";

  const onStart = () => {
    setDragging(true);
    setPieceDragging(true);
    setSelectedPiece(piecePosition);
  };

  const onStop = () => {
    makeMove();
    setPieceDragging(false);
    setDragging(false);
  };

  return (
    <Draggable
      draggable
      disabled={!draggable}
      nodeRef={pieceRef}
      position={pos}
      onStart={onStart}
      onStop={onStop}
      bounds="body"
    >
      <div
        ref={pieceRef}
        className={`border-b-4 w-10/12 rounded-full aspect-square box-border
         ${borderColor}
         ${bgColor}
         ${draggable && "hover:cursor-grab"}
         ${pieceDragging && "pointer-events-none"}
         ${pieceDragging && "z-50"}
         ${pieceDragging && "drop-shadow-2xl"}`}
      />
    </Draggable>
  );
}

export default Piece;
