import * as React from 'react';
import { IScore } from 'src/models/IScore';

export interface ILeaderboardProps{
    score:IScore;
    scoreValue:number;
    typeOfGame:string;
}
const Leaderboard=(props:ILeaderboardProps)=>
{
    if (props.typeOfGame=="solo")
    {
    return(
        <div key={props.score.id} >  
            <span className='leaderboard'>Nume: {`${props.score.team.players[0].name}`} Score: {`${props.scoreValue}`}</span>
        </div>
    )
    }
    else{
        return(
            <div key={props.score.id} >  
                <span className='leaderboard'>Nume: {`${props.score.team.name}`} Score: {`${props.scoreValue}`}</span>
            </div>
        )
    }
    
}
export default Leaderboard;