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
        var position=0;
        for (var i=0;i<this.props.listSelectedGames.length;i++)
        {
            if (this.props.listSelectedGames[i].id==this.props.game.id)
            {
                position=i;                
            }
        }
        if (position==0)
        {
            this.props.listSelectedGames.push(this.props.game);
        }
        else{
            this.props.listSelectedGames.splice(i, 1)
        }
        console.log(this.props.listSelectedGames);
    }
    render()
        {
            return(
                <div key={this.props.game.id}  className="gameLi" >
                <div className="checkbox">
                <input type="checkbox" onChange={this.onChange} defaultChecked={false}/>
                </div>
                <div className="imgBtnDiv">
                <button className="gameBox" onClick={() => { this.props.selectGame(this.props.game) }}>
                <article className="gameArticle">
                <img src={require('src/Resurces/gameLogo.png')}/>
                </article>               
                <section>
                Name: {`${this.props.game.name}`}
                </section>
                </button>
                </div>
                </div>
    ); 

}
}
