import Field from "./Field";

function Board() {
  return (
    <div>
      {[...Array(16)].map((e, i) => (
        <div key={i}>
          <div className="flex">
            {[...Array(16)].map((e, j) => (
              <Field key={i} color={(i + j) % 2 ? "white" : "black"} />
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}

export default Board;
