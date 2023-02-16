import { useEffect, useState } from "react";
import Chessboard from "./Chessboard/Chessboard";

const NewGame = () => {
  const [gameId, setGameId] = useState("");

  useEffect(() => {
    fetch("http://localhost:5093/creategame").then((response) => {
      response.json().then((data) => {
        setGameId(data.gameId);
      });
    });
  }, []);

  const makeMove = (move) => {
    const data = {
      "move": move,
      "gameId": gameId,
    };
    
    fetch("http://localhost:5093/moverequest", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
  };

  return (
    <div>
      <Chessboard makeMove={makeMove} />
    </div>
  );
};

export default NewGame;
