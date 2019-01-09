import * as React from 'react';
import { IScore } from 'src/models/IScore';
import "./Leaderboard.css";

export interface ILeaderboardProps{
    position:any;
    score:IScore;
    scoreValue:number;
    typeOfGame:string;
}
const Leaderboard=(props:ILeaderboardProps)=>
{
    if (props.typeOfGame=="solo")
    {
       console.log("aici");
    return(
        <div key={props.score.id} className="leaderboardContiner" >  
            <span className='leaderboard'>{`${props.position+1}`}. Name: {`${props.score.team.players[0].name}`} - Score: {`${props.scoreValue}`}</span>
        </div>
    )
    }
    else{
        console.log("aici");
        return(
            <div key={props.score.id} className="leaderboardContiner" >  
                <span className='leaderboard'>{`${props.position+1}`}. Name: {`${props.score.team.name}`} - Score: {`${props.scoreValue}`}</span>
            </div>
        )
    }
    
}
export default Leaderboard;