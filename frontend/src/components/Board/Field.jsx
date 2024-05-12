function Field({ color, dragging, children, fieldPosition, setSelectedField }) {
  const bgColor = color === "white" ? "bg-platinum" : "bg-neutral-800";

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
      {children}
    </div>
  );
}

export default Field;
