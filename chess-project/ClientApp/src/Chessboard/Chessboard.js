import React from "react";
import Square from "./Square";
import "./Chessboard.css";
import {useState} from "react";

function Chessboard({makeMove, legalMoves}) {
    const [pieces, setPieces] = useState([{type: "r", color: "b", position: {x: 0, y: 0}}, {
        type: "n",
        color: "b",
        position: {x: 1, y: 0}
    }, {type: "b", color: "b", position: {x: 2, y: 0}}, {type: "q", color: "b", position: {x: 3, y: 0}}, {
        type: "k",
        color: "b",
        position: {x: 4, y: 0}
    }, {type: "b", color: "b", position: {x: 5, y: 0}}, {type: "n", color: "b", position: {x: 6, y: 0}}, {
        type: "r",
        color: "b",
        position: {x: 7, y: 0}
    }, {type: "p", color: "b", position: {x: 0, y: 1}}, {type: "p", color: "b", position: {x: 1, y: 1}}, {
        type: "p",
        color: "b",
        position: {x: 2, y: 1}
    }, {type: "p", color: "b", position: {x: 3, y: 1}}, {type: "p", color: "b", position: {x: 4, y: 1}}, {
        type: "p",
        color: "b",
        position: {x: 5, y: 1}
    }, {type: "p", color: "b", position: {x: 6, y: 1}}, {type: "p", color: "b", position: {x: 7, y: 1}}, {
        type: "r",
        color: "w",
        position: {x: 0, y: 7}
    }, {type: "n", color: "w", position: {x: 1, y: 7}}, {type: "b", color: "w", position: {x: 2, y: 7}}, {
        type: "q",
        color: "w",
        position: {x: 3, y: 7}
    }, {type: "k", color: "w", position: {x: 4, y: 7}}, {type: "b", color: "w", position: {x: 5, y: 7}}, {
        type: "n",
        color: "w",
        position: {x: 6, y: 7}
    }, {type: "r", color: "w", position: {x: 7, y: 7}}, {type: "p", color: "w", position: {x: 0, y: 6}}, {
        type: "p",
        color: "w",
        position: {x: 1, y: 6}
    }, {type: "p", color: "w", position: {x: 2, y: 6}}, {type: "p", color: "w", position: {x: 3, y: 6}}, {
        type: "p",
        color: "w",
        position: {x: 4, y: 6}
    }, {type: "p", color: "w", position: {x: 5, y: 6}}, {type: "p", color: "w", position: {x: 6, y: 6}}, {
        type: "p",
        color: "w",
        position: {x: 7, y: 6}
    },]);


    // Handle the movement of a piece to a new position.
    const handlePieceMove = (piece, newPosition) => {
        // loop through each legal move and return the move that matches the piece and the new position.
        const move = legalMoves.find((move) => {
            return move.from === piece.position.y * 8 + piece.position.x && move.to === newPosition.y * 8 + newPosition.x;
        });

        if (!move) return;
        

        // Notify the parent component (NewGame) about the move.
        makeMove(move);
        
        console.log(move)
        
        // Remove the captured piece, if any.
        const removeCapturedPiece = pieces.filter((p) => {
            // If this is an en passant move, remove the captured pawn
            if (move.isEnPassant)
                return (p.position.y * 8 + p.position.x) !== move.capturedSquare;
            return (p.position.y * 8 + p.position.x) !== move.to;
        });

        // Update the positions of the pieces.
        const updatedPieces = removeCapturedPiece.map((p) => {
            if (p === piece)
                return {...p, position: newPosition};
            // Update the rook's position if the move is a castling move
            if (move.isCastling) {
                // Check if the current piece is a rook and has the same color as the king
                if (p.type === 'r' && p.color === piece.color) {
                    // If the rook's current position matches the castlingRookFrom square, update its position to the castlingRookTo square
                    if (p.position.y * 8 + p.position.x === move.castlingRookFrom) {
                        const updatedRookPosition = {
                            x: move.castlingRookTo % 8,
                            y: Math.floor(move.castlingRookTo / 8),
                        };
                        return {...p, position: updatedRookPosition};
                    }
                }
            }
            return p;
        });

        // Set the updated state of the pieces.
        setPieces([...updatedPieces]);
    };

    // Keep track of the currently selected piece.
    const [selectedPiece, setSelectedPiece] = useState(null);

    // Handle the onClick event on a square.
    const handleClick = (e, piece, position) => {
        if (selectedPiece) {
            if (selectedPiece !== piece)
                // Move the selected piece to the new position.
                handlePieceMove(selectedPiece, position);
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
                    onPieceMove={handlePieceMove}
                    onClick={(e) => handleClick(e, piece, position)}
                />);
            })}
        </div>))}
    </div>);
}

export default Chessboard;