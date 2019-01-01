import * as React from 'react';
import { IGame } from 'src/models/IGame';
import "./Game.css";

export interface IGameProps{
    game: IGame;
    listSelectedGames: IGame[];
    selectGame(game: IGame): void;
}
export default class Game extends React.Component<IGameProps, any>{
    constructor(props: any) {
        super(props);
        this.state = {
        }
    }
    onChange=()=>{
        this.props.listSelectedGames.push(this.props.game);
        console.log(this.props.listSelectedGames);
    }
    render()
        {
            return(
                <div key={this.props.game.id}  className="gameLi" >
                <input type="checkbox" onChange={this.onChange} defaultChecked={false}/>
                <button className="gameBox" onClick={() => { this.props.selectGame(this.props.game) }}>
                <article>
                <img src={require('src/Resurces/game.png')}/>
                </article>
                <section>
                Game name: {`${this.props.game.name}`}
                </section>
                </button>
                </div>
    ); 

}
}
