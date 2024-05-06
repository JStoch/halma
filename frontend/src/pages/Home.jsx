import Button from "../components/Button";
import Board from "../components/Board";

function Home() {
  return (
    <div className="flex-1 flex">
      <div className="basis-2/3 flex flex-col items-center p-8">
        <h1 className="text-8xl text-white text-shadow shadow-neutral-800 select-none">
          Halma.com
        </h1>
        <h2 className="text-2xl mb-8 text-white text-shadow shadow-neutral-800 select-none">
          The biggest (and only) halma server with competetive features
        </h2>
        <Board />
      </div>
      <div className="flex flex-col p-5 gap-6 justify-center">
        <Button
          className="bg-lime-300 hover:bg-lime-200 text-2xl px-8 py-8 bor"
          value="Ranked game"
        />
        <Button
          className="bg-cyan-300 hover:bg-cyan-200 text-2xl px-8 py-8 bor"
          value="Custom game"
        />
        <Button
          className="bg-amber-300 hover:bg-amber-200 text-2xl px-8 py-8 bor"
          value="Learn halma rules"
          url="https://en.wikipedia.org/wiki/Halma#Rules"
        />
      </div>
    </div>
  );
}

export default Home;
