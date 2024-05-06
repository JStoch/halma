function Field({ color }) {
  const bgColor = color === "white" ? "bg-platinum" : "bg-neutral-800";

  return <div className={`${bgColor} w-7 h-7`}></div>;
}

export default Field;
