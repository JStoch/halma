function Field({ color, size }) {
  const bgColor = color === "white" ? "bg-platinum" : "bg-neutral-800";
  const cellWidth = size === "big" ? "w-10" : "w-7";
  const cellHeight = size === "big" ? "h-10" : "h-7";

  return <div className={`${bgColor} ${cellWidth} ${cellHeight}`}></div>;
}

export default Field;
