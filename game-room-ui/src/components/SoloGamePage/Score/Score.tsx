import * as React from 'react';
import { IScore } from 'src/models/IScore';
import ScoreService from 'src/services/ScoreService';

export interface IScoreProps {
    score: IScore;
    typeOfGame: string;
    scoreValue: number;
}
export interface IScoreState {
    id: string;
    points: number;
    currentScore: number;
    refresh: boolean;

}
export default class Score extends React.Component<IScoreProps, IScoreState>
{
    constructor(props: IScoreProps) {
        super(props);
        this.state = {
            id: '',
            points: 0,
            currentScore: 0,
            refresh: false
        }
    }
    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: IScoreState) => (
            {

                ...prevState,
                [name]: value
            }
        ));
    }
    componentDidMount() {
        console.log(this.state.id);
        let idScore = this.props.score.id;
        let score = this.props.score.value;
        if (idScore != undefined) {
            this.setState({ id: idScore, currentScore: score })
        }

    }
    updateScore = () => {
        var currentScore = this.state.currentScore + "";
        var addPoints = this.state.points + "";
        console.log(this.state.id);
        ScoreService.getScoreById(this.state.id).then((score: IScore) => {
            var total = parseInt(currentScore) + parseInt(addPoints);
            score.value = total;
            ScoreService.updateScore(this.state.id, score);
            this.setState({ points: 0, currentScore: total });
        })
    }
    render() {

        if (this.props.typeOfGame == "solo") {
            return (
                <div key={this.props.score.id}>
                    <span className="score">{`${this.props.score.team.players[0].name}-${this.props.score.value}`} </span>
                    <input type="number" name="points" onChange={this.handleChange} value={this.state.points} />
                    <button onClick={this.updateScore}>Update Score</button>
                </div>
            );
        }
        else {
            return (

                <div>
                    <span className="score">{`${this.props.score.team.name}-${this.props.score.value}`}</span>
                    <input type="number" name="points" onChange={this.handleChange} value={this.state.points} />
                    <button onClick={this.updateScore}>Update Score</button>
                </div>
            );
        }
    }

}
