import * as React from 'react';
import { IGame } from 'src/models/IGame';
import "./Game.css";

export interface IGameProps{
    game: IGame;
    selectGame(game: IGame): void;
}
const Game = (props:IGameProps) =>
{
    return(
        <div key={props.game.id} className="gameLi" >
            <span className='game' onClick={() => { props.selectGame(props.game) }}>Game name: {`${props.game.name}`}</span>
        </div>
    ); 

}
export default Game;
