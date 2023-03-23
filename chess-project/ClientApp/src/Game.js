import {useEffect, useState} from "react";
import Chessboard from "./Chessboard/Chessboard";

const NewGame = () => {
    const [gameId, setGameId] = useState();
    const [legalMoves, setLegalMoves] = useState([]);
    const makeMove = (move) => {
        fetch(`makemove?guid=${gameId}&from=${move.start.x + move.start.y * 8}&to=${move.end.x + move.end.y * 8}`)
            .then(getLegalMoves);
    };
    
    const getLegalMoves = () => {
        fetch(`getlegalmoves?guid=${gameId}`)
            .then(response => response.json())
            .then(data => setLegalMoves(data));
    };

    useEffect(() => {
        fetch('creategameid')
            .then(response => response.json())
            .then(data => setGameId(data.guid));
    }, []);

    useEffect(() => {
        if (gameId) {
            getLegalMoves();
        }
    }, [gameId]);
    
    return (
        <div>
            <Chessboard makeMove={makeMove} legalMoves={legalMoves}/>
        </div>
    );
};

export default NewGame;
