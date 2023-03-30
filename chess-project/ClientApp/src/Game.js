import {useEffect, useState} from "react";
import Chessboard from "./Chessboard/Chessboard";

const NewGame = () => {
    const [gameId, setGameId] = useState();
    const [legalMoves, setLegalMoves] = useState([]);
    const [playerMove, setPlayerMove] = useState(true);
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

    const posToSquare = (pos) => {
        return (7 - pos.y) * 8 + pos.x;
    }

    const findPiece = (data) => {
        return pieces.find((p) => {
            return posToSquare(p.position) === data.from;
        });
    }

    const squareToPos = (square) => {
        return {
            x: square % 8,
            y: 7 - Math.floor(square / 8),
        };
    }

    const handlePlayerMove = (piece, newPosition) => {
        // loop through each legal move and return the move that matches the piece and the new position.
        const move = legalMoves.find((move) => {
            return move.from === posToSquare(piece.position) && move.to === posToSquare(newPosition);
        });

        if (!move) return;

        handlePieceMove(move);

        makeMove(move)
    }

    // Handle the movement of a piece to a new position.
    const handlePieceMove = (move) => {
        const piece = findPiece(move);
        const newPosition = squareToPos(move.to);
        // Remove the captured piece, if any.
        const removeCapturedPiece = pieces.filter((p) => {
            // If this is an en passant move, remove the captured pawn
            if (move.isEnPassant)
                return (posToSquare(p.position)) !== move.capturedSquare;
            return (posToSquare(p.position)) !== move.to;
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
                    if (posToSquare(p.position) === move.castlingRookFrom) {
                        const updatedRookPosition = squareToPos(move.castlingRookTo);
                        return {...p, position: updatedRookPosition};
                    }
                }
            }
            return p;
        });

        // Set the updated state of the pieces.
        setPieces([...updatedPieces]);
    };

    const makeMove = async (move) => {
        await fetch(`makemove?guid=${gameId}&from=${move.from}&to=${move.to}`)
        await setPlayerMove(false)
    };

    const getLegalMoves = () => {
        fetch(`getlegalmoves?guid=${gameId}`)
            .then(response => response.json())
            .then(data => setLegalMoves(data));
    };

    useEffect(() => {
        if (gameId) {
            if (playerMove)
                getLegalMoves()
            else {
                fetch(`getaimove?guid=${gameId}`)
                    .then(response => response.json())
                    .then(data => handlePieceMove(data))
                    .then(() => setPlayerMove(true))
            }
        }
    }, [playerMove]);


        useEffect(() => {
            fetch('creategameid')
                .then(response => response.json())
                .then(data => setGameId(data.guid));
        }, []);

        useEffect(() => {
            if (gameId)
                getLegalMoves();
        }, [gameId]);


        return (
            <div>
                <Chessboard handlePlayerMove={handlePlayerMove} pieces={pieces}/>
            </div>
        );
    };

    export default NewGame;
