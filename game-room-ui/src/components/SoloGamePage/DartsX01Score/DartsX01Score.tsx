import * as React from 'react';
import { IScore } from 'src/models/IScore';
import { IDartsX01 } from 'src/models/IDartsX01';
import DartsX01Service from 'src/services/DartsX01Service';
import './DartsX01Score.css';

export interface IDartsX01Props {
    score: IScore;
    typeOfGame: string;
    scoreValue: number;
    dartsX01: IDartsX01;
    onChangeDartsX01(): any;
}
export interface IDartsX01State {
    id: string;
    points: number;
    currentScore: number;

}
export default class DartsX01Score extends React.Component<IDartsX01Props, IDartsX01State>
{
    constructor(props: IDartsX01Props) {
        super(props);
        this.state = {
            id: '',
            points: 0,
            currentScore: 0
        }
    }
    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: IDartsX01State) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }
    componentDidMount() {
        let idDartsX01 = this.props.dartsX01.id;
        let score = this.props.dartsX01.stateScore;
        if (idDartsX01 != undefined) {
            this.setState({ id: idDartsX01, currentScore: score })
        }
    }
    componentWillReceiveProps() {
        let idDartsX01 = this.props.dartsX01.id;
        let score = this.props.dartsX01.stateScore;
        if (idDartsX01 != undefined) {
            this.setState({ id: idDartsX01, currentScore: score })
        }
    }
    updateScore = () => {
        var currentScore = this.state.currentScore + "";
        var addPoints = this.state.points + "";
        console.log(currentScore);
        var id = this.state.id;
        if (parseInt(currentScore) >= parseInt(addPoints)) {
            var total = parseInt(currentScore) - parseInt(addPoints);
            this.setState({ points: 0, currentScore: total });
            DartsX01Service.getDartsX01ById(id).then((dartsX01: IDartsX01) => {
                dartsX01.stateScore = total;
                DartsX01Service.updateDartsX01(this.state.id, dartsX01).then(()=>{
                    this.props.onChangeDartsX01();
                });
            })
        }
    }
    render() {
        if (this.props.typeOfGame == "solo") {
            return (
                <div key={this.props.score.id} className="dartsX01Form">
                    <div className="dartsX01Score">
                    <span className="score">{`${this.props.score.team.players[0].name}-${this.props.dartsX01.stateScore}`} </span>
                    </div>
                    <input type="number" name="points" onChange={this.handleChange} value={this.state.points} className="dartsX01Score" />
                    <button className="updateDartsX01" onClick={this.updateScore}>Update Score</button>
                </div>
            );
        }
        else {
            return (

                <div key={this.props.score.id} className="dartsX01Form">
                    <div className="dartsX01Score">
                    <span className="score">{`${this.props.score.team.name}-${this.props.dartsX01.stateScore}`}</span>
                    </div>
                    <input type="number" name="points" onChange={this.handleChange} value={this.state.points} className="dartsX01Score" />
                    <button className="updateDartsX01" onClick={this.updateScore}>Update Score</button>
                </div>
            );
        }
    }

}
