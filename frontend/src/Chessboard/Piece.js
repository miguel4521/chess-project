import React, { useRef, useState, useEffect } from "react";
import "./Piece.css";

function Piece({ type, color, onMove, position }) {
  // Create a variable to store the name of the chess piece
  let pieceName = `${color}${type}`;

  const chessboard = document.getElementById("chessboard");

  // Create a reference to the div element that contains the chess piece image
  const pieceRef = useRef(null);

  // Use state to store the current position of the cursor
  const [cursorPosition, setCursorPosition] = useState({ x: 0, y: 0 });

  // Use state to store whether the chess piece is being dragged
  const [isDragging, setIsDragging] = useState(false);

  // Use state to store the current position of the element
  const [elementPosition, setElementPosition] = useState({ x: 0, y: 0 });

  const isMouseOverChessboard = () => {
    // Check if the mouse is hovering over the chessboard
    const chessboardRect = chessboard.getBoundingClientRect();
    return (
      cursorPosition.x >= chessboardRect.left &&
      cursorPosition.x <= chessboardRect.right &&
      cursorPosition.y >= chessboardRect.top &&
      cursorPosition.y <= chessboardRect.bottom
    );
  };

  // Use a useEffect hook to track the mouse position
  useEffect(() => {
    const handleMouseMove = (e) => {
      setCursorPosition({
        x: e.clientX,
        y: e.clientY,
      });
    };

    window.addEventListener("mousemove", handleMouseMove);
    return () => {
      window.removeEventListener("mousemove", handleMouseMove);
    };
  }, []);

  // Use a useEffect hook to update the element position when the mouse moves
  useEffect(() => {
    if (isDragging) {
      if (!isMouseOverChessboard())
        // If the mouse is not hovering over the chessboard, then do not update the position
        return;
      // Get the dimensions of the element
      const elementRect = pieceRef.current.getBoundingClientRect();

      // Calculate the new top and left positions of the element based on the mouse position and the element dimensions
      const newTop =
        cursorPosition.y -
        elementRect.height / 2 -
        elementRect.height * position.y -
        chessboard.offsetTop;
      const newLeft =
        cursorPosition.x -
        elementRect.width / 2 -
        elementRect.width * position.x -
        chessboard.offsetLeft;

      // Update the element position state
      setElementPosition({
        x: newLeft,
        y: newTop,
      });
    }
  }, [cursorPosition, isDragging]);

  // This is to handle the case where the mouse is released outside of the chessboard
  useEffect(() => {
    const handleMouseUp = () => {
      setElementPosition({ x: 0, y: 0 });
      setIsDragging(false);
    };
    window.addEventListener("mouseup", handleMouseUp);

    return () => {
      window.removeEventListener("mouseup", handleMouseUp);
    };
  }, []);

  // Define a function to handle mouse down events
  const handleMouseDown = () => {
    setIsDragging(true);
  };

  // Define a function to handle mouse up events
  const handleMouseUp = () => {
    setIsDragging(false);

    // Get the dimensions of the element
    const elementRect = pieceRef.current.getBoundingClientRect();

    // Calculate the new position of the element based on the mouse position and the element dimensions
    const x = Math.floor(
      (cursorPosition.x - chessboard.offsetLeft) / elementRect.height
    );
    const y = Math.floor(
      (cursorPosition.y - chessboard.offsetTop) / elementRect.width
    );

    // If the new position is the same as the starting position, reset the element position
    if (position.x === x && position.y === y) {
      setElementPosition({ x: 0, y: 0 });
      return;
    }

    // Otherwise, update the position of the piece
    onMove({ x: x, y: y });
  };

  return (
    <div
      className="chess-piece"
      style={{
        top: `${elementPosition.y}px`,
        left: `${elementPosition.x}px`,
        zIndex: isDragging ? 1000 : 0,
      }}
    >
      <div
        className="image"
        onMouseDown={handleMouseDown}
        onMouseUp={handleMouseUp}
        ref={pieceRef}
        style={{
          backgroundImage: `url(Assets/Pieces/${pieceName}.png)`,
        }}
      />
    </div>
  );
}

export default Piece;
