import { React, useState, useEffect } from "react";
import Piece from "./Piece";
import "./Square.css";

function Square({ position, piece, onPieceMove, onClick }) {
  let color =
    (position.x + position.y) % 2 === 0 ? "light-square" : "dark-square";

  return (
    <div className={`square ${color}`} onClick={onClick}>
      {piece ? (
        <Piece
          type={piece.type}
          color={piece.color}
          onMove={(newPosition) => onPieceMove(piece, newPosition)}
          position={piece.position}
        />
      ) : null}
    </div>
  );
}

export default Square;
