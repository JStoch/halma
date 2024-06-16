import { useState } from "react";
import Button from "../Button";

function EndGamePopup({ result }) {
  const [isOpen, setIsOpen] = useState(true);

  const closePopup = () => {
    setIsOpen(false);
  };

  if (!isOpen) {
    return null;
  }

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
      <div className="bg-platinum p-6 rounded-md shadow-lg text-center">
        <h2 className="text-2xl mb-4">
          {result === "win" ? "You won!" : "You lost!"}
        </h2>
        <Button
          onClick={closePopup}
          className="bg-red-500 hover:bg-red-400"
          value="Close"
        />
        {/* <button
          onClick={closePopup}
          className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-700"
        >
          Close
        </button> */}
      </div>
    </div>
  );
}

export default EndGamePopup;
