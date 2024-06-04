function Field({
  color,
  dragging,
  children,
  fieldPosition,
  setSelectedField,
  highlighted,
  movePossible,
}) {
  let bgColor = color === "white" ? "bg-platinum" : "bg-neutral-800";
  bgColor = highlighted ? "bg-yellow-500" : bgColor;

  return (
    <div
      className={`basis-1/16 aspect-square flex justify-center items-center
      ${bgColor}
      ${
        dragging &&
        "cursor-grabbing hover:outline hover:outline-3 hover:outline-offset-[-3px] hover:outline-sky-500"
      }`}
      onMouseOver={() => {
        if (dragging) {
          setSelectedField(fieldPosition);
        }
      }}
    >
      {movePossible ? (
        <div className="w-3 h-3 rounded-full bg-yellow-500"></div>
      ) : (
        children
      )}
    </div>
  );
}

export default Field;
