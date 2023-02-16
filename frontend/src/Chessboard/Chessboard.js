import React from "react";
import Square from "./Square";
import "./Chessboard.css";
import { useState } from "react";

function Chessboard({makeMove}) {
  const [pieces, setPieces] = useState([
    { type: "r", color: "b", position: { x: 0, y: 0 } },
    { type: "n", color: "b", position: { x: 1, y: 0 } },
    { type: "b", color: "b", position: { x: 2, y: 0 } },
    { type: "q", color: "b", position: { x: 3, y: 0 } },
    { type: "k", color: "b", position: { x: 4, y: 0 } },
    { type: "b", color: "b", position: { x: 5, y: 0 } },
    { type: "n", color: "b", position: { x: 6, y: 0 } },
    { type: "r", color: "b", position: { x: 7, y: 0 } },
    { type: "p", color: "b", position: { x: 0, y: 1 } },
    { type: "p", color: "b", position: { x: 1, y: 1 } },
    { type: "p", color: "b", position: { x: 2, y: 1 } },
    { type: "p", color: "b", position: { x: 3, y: 1 } },
    { type: "p", color: "b", position: { x: 4, y: 1 } },
    { type: "p", color: "b", position: { x: 5, y: 1 } },
    { type: "p", color: "b", position: { x: 6, y: 1 } },
    { type: "p", color: "b", position: { x: 7, y: 1 } },
    { type: "r", color: "w", position: { x: 0, y: 7 } },
    { type: "n", color: "w", position: { x: 1, y: 7 } },
    { type: "b", color: "w", position: { x: 2, y: 7 } },
    { type: "q", color: "w", position: { x: 3, y: 7 } },
    { type: "k", color: "w", position: { x: 4, y: 7 } },
    { type: "b", color: "w", position: { x: 5, y: 7 } },
    { type: "n", color: "w", position: { x: 6, y: 7 } },
    { type: "r", color: "w", position: { x: 7, y: 7 } },
    { type: "p", color: "w", position: { x: 0, y: 6 } },
    { type: "p", color: "w", position: { x: 1, y: 6 } },
    { type: "p", color: "w", position: { x: 2, y: 6 } },
    { type: "p", color: "w", position: { x: 3, y: 6 } },
    { type: "p", color: "w", position: { x: 4, y: 6 } },
    { type: "p", color: "w", position: { x: 5, y: 6 } },
    { type: "p", color: "w", position: { x: 6, y: 6 } },
    { type: "p", color: "w", position: { x: 7, y: 6 } },
  ]);

  const handlePieceMove = (piece, newPosition) => {
    makeMove({start: piece.position, end: newPosition});
    const removeCapturedPiece = pieces.filter((p) => {
      return JSON.stringify(p.position) !== JSON.stringify(newPosition);
    });
    const updatedPieces = removeCapturedPiece.map((p) => {
      if (p === piece) {
        return { ...p, position: newPosition };
      }
      return p;
    });

    setPieces([...updatedPieces]);
  };

  const [selectedPiece, setSelectedPiece] = useState(null);

  const handleClick = (e, piece, position) => {
    if (selectedPiece) {
      if (selectedPiece !== piece) handlePieceMove(selectedPiece, position)
        setSelectedPiece(null);
    } else
      setSelectedPiece(piece);
  };

  return (
    <div id="chessboard">
      {[0, 1, 2, 3, 4, 5, 6, 7].map((col) => (
        <div key={col} className="column">
          {[0, 1, 2, 3, 4, 5, 6, 7].map((row) => {
            const position = { x: row, y: col };
            const piece = pieces.find(
              (p) => JSON.stringify(p.position) === JSON.stringify(position)
            );
            return (
              <Square
                key={`${position.x}${position.y}`}
                position={position}
                piece={piece}
                onPieceMove={handlePieceMove}
                onClick={(e) => handleClick(e, piece, position)}
              />
            );
          })}
        </div>
      ))}
    </div>
  );
}

export default Chessboard;
