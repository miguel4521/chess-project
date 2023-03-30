import React from "react";
import Square from "./Square";
import "./Chessboard.css";
import {useState} from "react";

function Chessboard({handlePlayerMove, pieces}) {
    // Keep track of the currently selected piece.
    const [selectedPiece, setSelectedPiece] = useState(null);

    // Handle the onClick event on a square.
    const handleClick = (e, piece, position) => {
        if (selectedPiece) {
            if (selectedPiece !== piece)
                // Move the selected piece to the new position.
                handlePlayerMove(selectedPiece, position);
            // Deselect the piece.
            setSelectedPiece(null);
        } else
            // Select the piece.
            setSelectedPiece(piece);
    };

    // Render the chessboard with columns and squares.
    return (<div id="chessboard">
        {[0, 1, 2, 3, 4, 5, 6, 7].map((col) => (<div key={col} className="column">
            {[0, 1, 2, 3, 4, 5, 6, 7].map((row) => {
                const position = {x: row, y: col};
                // Find the piece at the current position.
                const piece = pieces.find((p) => JSON.stringify(p.position) === JSON.stringify(position));
                return (<Square
                    key={`${position.x}${position.y}`}
                    position={position}
                    piece={piece}
                    onPieceMove={handlePlayerMove}
                    onClick={(e) => handleClick(e, piece, position)}
                />);
            })}
        </div>))}
    </div>);
}

export default Chessboard;