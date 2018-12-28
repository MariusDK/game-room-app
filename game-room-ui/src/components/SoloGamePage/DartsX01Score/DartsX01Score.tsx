import * as React from 'react';
import { IScore } from 'src/models/IScore';
import { IDartsX01 } from 'src/models/IDartsX01';
import DartsX01Service from 'src/services/DartsX01Service';

export interface IDartsX01Props {
    score: IScore;
    typeOfGame: string;
    scoreValue:number;
    dartsX01: IDartsX01;
}
export interface IDartsX01State {
    id:string;
    points:number;
    currentScore:number;

}
export default class DartsX01Score extends React.Component<IDartsX01Props,IDartsX01State>
{
    constructor(props:IDartsX01Props)
    {
        super(props);
        this.state = {
            id:'',
            points:0,
            currentScore:0
        }
    }
    handleChange = (e: any) => {
        const { name,value } = e.target;
        this.setState((prevState: IDartsX01State)=>(
            {    
                ...prevState,
                [name]:value
            }
        ));        
    }
    componentDidMount()
    {
        console.log(this.state.id);
        let idDartsX01 = this.props.dartsX01.id;
        let score = this.props.dartsX01.stateScore;
        if (idDartsX01!=undefined)
        {
            this.setState({id:idDartsX01,currentScore:score})            
        }

    }
    updateScore=()=>
    {
        var currentScore = this.state.currentScore+"";
        var addPoints = this.state.points+"";
        console.log(this.state.id);
        var id = this.state.id;
        console.log(id);
        DartsX01Service.getDartsX01ById(id).then((dartsX01:IDartsX01)=>{
            console.log(dartsX01);
            var total = parseInt(currentScore)-parseInt(addPoints);
            console.log(total)
            dartsX01.stateScore = total;
            
            DartsX01Service.updateDartsX01(this.state.id,dartsX01);
            this.setState({points:0,currentScore:total});
        })
    }
    render()
    {
    if (this.props.typeOfGame=="solo")
    {
        return (
            <div key={this.props.score.id}>
            <span className="score">{`${this.props.score.team.players[0].name}-${this.props.dartsX01.stateScore}`} </span>
            <input type="number" name="points" onChange={this.handleChange} value={this.state.points} />
            <button onClick={this.updateScore}>Update Score</button>
            <button>Delete Player</button>
            </div>
        );
    }
    else{
    return (
        
        <div>
            <span className="score">{`${this.props.score.team.name}-${this.props.dartsX01.stateScore}`}</span>
            <input type="number" value={this.state.points}/>
            <button>Update Score</button>
            <button>Delete Team</button>
        </div>
    );
    }
}
    
}