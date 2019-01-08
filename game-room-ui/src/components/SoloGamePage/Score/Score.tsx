import * as React from 'react';
import { IScore } from 'src/models/IScore';
import ScoreService from 'src/services/ScoreService';
import "./Score.css"

export interface IScoreProps {
    score: IScore;
    typeOfGame: string;
    scoreValue: number;
    onChange(): any;
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
        let idScore = this.props.score.id;
        let score = this.props.score.value;
        if (idScore != undefined) {
            this.setState({ id: idScore, currentScore: score })
        }
    }
    updateScore = () => {
        var currentScore = this.state.currentScore + "";
        var addPoints = this.state.points + "";
        var total = parseInt(currentScore) + parseInt(addPoints);
        this.setState({ points: 0, currentScore: total });
        ScoreService.getScoreById(this.state.id).then((score: IScore) => {
            score.value = total;
            ScoreService.updateScore(this.state.id, score);
            this.props.onChange();

        })
    }
    render() {

        if (this.props.typeOfGame == "solo") {
            return (
                <div key={this.props.score.id} className="scoreForm">
                    <div className="normalScore">
                    <span className="score">{`${this.props.score.team.players[0].name} - ${this.state.currentScore}`} </span>
                    </div>
                    <input type="number" name="points" onChange={this.handleChange} value={this.state.points} />
                    <button className="updateNormalScore" onClick={this.updateScore}>Update Score</button>
                </div>
            );
        }
        else {
            return (

                <div key={this.props.score.id} className="scoreForm">
                    <div className="multiScore">
                    <span className="score">{`${this.props.score.team.name} - ${this.state.currentScore}`}</span>
                    </div>
                    <input type="number" name="points" onChange={this.handleChange} value={this.state.points} />
                    <button className="updateNormalScore" onClick={this.updateScore}>Update Score</button>
                </div>
            );
        }
    }

}
