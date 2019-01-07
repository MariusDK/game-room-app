import * as React from 'react';
import './Suggestions.css';
import { IPlayer } from 'src/models/IPlayer';

export interface IPlayerProps{
  player: IPlayer;
  removePlayerFromList(player:IPlayer):any;
}

export default class PlayerCard extends React.Component<IPlayerProps, any>{
  constructor(props: any) {
    super(props);
    this.state = {
    }
}
removePlayer=(player:IPlayer)=>
{
  this.props.removePlayerFromList(player);
}

render()
{
  return(
    <div key={this.props.player.id} className="personCard">
    <div className="button-block">
    <button className="cancelButton" onClick={()=>this.removePlayer(this.props.player)} >
    <i className="mark x"></i>
    <i className="mark xx"></i>
    </button>
    </div>
    <article>
      <img src={require('src/Resurces/person.png')}></img>
      </article>
      <section>
      Name: {this.props.player.name}
      </section>
      </div>
  )
}
}
