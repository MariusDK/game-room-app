import * as React from 'react';
import './TeamCard.css';
import { ITeam } from 'src/models/ITeam';

export interface ITeamProps{
  team : ITeam;
  removeTeamFromList(team:ITeam):any;
}
export default class TeamCard extends React.Component<ITeamProps,any>{
  constructor(props: any)
  {
    super(props);
    this.state = {
    }
  }
  removeTeam=(team:ITeam)=>
  {
    this.props.removeTeamFromList(team);
  }
render()
{
  return(
      <div key={this.props.team.id} className="teamCard">
      <div className="button-block">
        <button className="cancelButton" onClick={()=>this.removeTeam(this.props.team)}>
        <i className="mark x"></i>
        <i className="mark xx"></i>
        </button>
      </div>
      <article>
        <img src={require('src/Resurces/team.png')}></img>
        </article>
        <section>
        Name: {this.props.team.name}
        </section>
        </div>
  )
  }
}
