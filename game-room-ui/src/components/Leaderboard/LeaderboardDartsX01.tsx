import * as React from 'react';
import { IScore } from 'src/models/IScore';
import { IDartsX01 } from 'src/models/IDartsX01';

export interface ILeaderboardProps{
    score:IScore;
    scoreValue:number;
    typeOfGame:string;
    dartsX01:IDartsX01;
}
const Leaderboard=(props:ILeaderboardProps)=>
{
    if (props.typeOfGame=="solo")
    {
    return(
        <div key={props.score.id} >  
            <span className='leaderboard'>Nume: {`${props.score.team.players[0].name}`} Score: {`${props.dartsX01.stateScore}`}</span>
        </div>
    )
    }
    else{
        return(
            <div key={props.score.id} >  
                <span className='leaderboard'>Nume: {`${props.score.team.name}`} Score: {`${props.dartsX01.stateScore}`}</span>
            </div>
        )
    }
    
}
export default Leaderboard;