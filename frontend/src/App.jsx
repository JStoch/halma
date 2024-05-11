import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Game from "./pages/Game";
import Sidebar from "./components/Sidebar";

function App() {
  return (
    <div className="h-screen bg-hex flex flex-row-reverse font-seri">
      <Sidebar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/game" element={<Game />} />
      </Routes>
    </div>
  );
}

export default App;
