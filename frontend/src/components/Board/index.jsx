import Field from "./Field";

function Board({ size }) {
  return (
    <div>
      {[...Array(16)].map((e, i) => (
        <div key={i}>
          <div className="flex">
            {[...Array(16)].map((e, j) => (
              <Field
                key={j}
                color={(i + j) % 2 ? "white" : "black"}
                size={size}
              />
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}

export default Board;
