import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Game from "./pages/Game";
import Sidebar from "./components/Sidebar";
import { useState } from "react";

function App() {
  const [user, setUser] = useState(null);
  const [profile, setProfile] = useState(null);

  return (
    <div className="h-screen bg-hex flex flex-row-reverse font-seri">
      <Sidebar
        setUser={setUser}
        setProfile={setProfile}
        user={user}
        profile={profile}
      />
      <Routes>
        <Route path="/" element={<Home profile={profile} />} />
        <Route path="/game" element={<Game profile={profile} />} />
      </Routes>
    </div>
  );
}

export default App;
