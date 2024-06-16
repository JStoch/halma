import Button from "../components/Button";
import Board from "../components/Board";
import { useNavigate } from "react-router-dom";

function Home({ profile }) {
  const navigate = useNavigate();

  const loggedIn = !!profile;

  return (
    <div className="flex-1 flex">
      <div className="basis-2/3 flex flex-col justify-center items-center p-8">
        <h1 className="text-8xl text-white text-shadow shadow-neutral-800 select-none">
          Halma.com
        </h1>
        <h2 className="text-2xl mb-8 text-white text-shadow shadow-neutral-800 select-none">
          The biggest (and only) halma server with competetive features
        </h2>
        <Board className="w-1/2" disable />
      </div>
      <div className="flex flex-col p-5 gap-6 justify-center">
        <Button
          className={`${
            loggedIn ? "bg-lime-300 hover:bg-lime-200" : ""
          } text-2xl px-8 py-8 bor`}
          value="Ranked game"
          onClick={(e) => {
            e.preventDefault();
            navigate("/game");
          }}
          disabled={!loggedIn}
        />
        <Button
          className={`${
            loggedIn ? "bg-cyan-300 hover:bg-cyan-200" : ""
          } text-2xl px-8 py-8 bor`}
          value="Custom game"
          onClick={(e) => {
            e.preventDefault();
            navigate("/game");
          }}
          disabled={!loggedIn}
        />
        <Button
          className="bg-amber-300 hover:bg-amber-200 text-2xl px-8 py-8 bor"
          value="Learn halma rules"
          href="https://en.wikipedia.org/wiki/Halma#Rules"
          target="_blank"
        />
      </div>
    </div>
  );
}

export default Home;
