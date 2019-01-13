import * as React from 'react';
import { IScore } from 'src/models/IScore';
import { IDartsCricket } from 'src/models/IDartsCricket';
import ScoreService from 'src/services/ScoreService';
import DartsCricketService from 'src/services/DartsCricketService';
import './DartsCricketScore.css';

export interface IDartsCricketProps {
    typeOfGame: string;
    dartsCricket1: IDartsCricket;
    dartsCricket2: IDartsCricket;
    onChange(score1: number, score2: number): any;
}
export interface IDartsCricketState {
    hits1: string[];
    hits2: string[];
    round: number;
    closeNumbers1: string[];
    closeNumbers2: string[];
    openNumbers1: string[];
    openNumbers2: string[];
    score1: number;
    score2: number;
    gameStatus: string;
}
export default class DartsCricketScore extends React.Component<IDartsCricketProps, IDartsCricketState>
{
    constructor(props: IDartsCricketProps) {
        super(props);
        this.state = {
            hits1: [],
            hits2: [],
            closeNumbers1: [],
            closeNumbers2: [],
            openNumbers1: [],
            openNumbers2: [],
            score1: 0,
            score2: 0,
            round: 1,
            gameStatus:''
        }
    }
    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: IDartsCricketState) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }
    componentDidMount() {
        let gameState = localStorage.getItem('gameState');
        this.setState({gameStatus:gameState});
        console.log(this.props.dartsCricket1.score.value);
        let hits1: string[] = this.props.dartsCricket1.hits;
        if (hits1 == null) {
            hits1 = [];
        }
        let hits2: string[] = this.props.dartsCricket2.hits;
        if (hits2 == null) {
            hits2 = [];
        }
        let closeNumbers1: string[] = this.props.dartsCricket1.closeNumbers;
        if (closeNumbers1 == null) {
            closeNumbers1 = [];
        }
        let closeNumbers2: string[] = this.props.dartsCricket2.closeNumbers;
        if (closeNumbers2 == null) {
            closeNumbers2 = [];
        }
        let openNumbers1: string[] = this.props.dartsCricket1.openNumbers;
        if (openNumbers1 == null) {
            openNumbers1 = [];
        }
        let openNumbers2: string[] = this.props.dartsCricket2.openNumbers;
        if (openNumbers2 == null) {
            openNumbers2 = [];
        }
        this.setState({
            hits1: hits1,
            hits2: hits2,
            closeNumbers1: closeNumbers1,
            closeNumbers2: closeNumbers2,
            openNumbers1: openNumbers1,
            openNumbers2: openNumbers2,
            score1: this.props.dartsCricket1.score.value,
            score2: this.props.dartsCricket2.score.value
        })
    }
    checkCloseNumber(val: string): boolean {
        if (this.state.round % 2 != 0) {
            let closeNumbers1: string[] = this.state.closeNumbers1;
            for (let i = 0; i < closeNumbers1.length; i++) {
                if (closeNumbers1[i] == val) {
                    return true;
                }
            }
            return false;
        }
        else {
            let closeNumbers2: string[] = this.state.closeNumbers2;
            for (let i = 0; i < closeNumbers2.length; i++) {
                if (closeNumbers2[i] == val) {
                    return true;
                }
            }
            return false;
        }
    }
    update() {
        let idScore1 = this.props.dartsCricket1.score.id;
        if (idScore1 != undefined) {
            ScoreService.getScoreById(idScore1).then((score1: IScore) => {
                score1.value = this.state.score1;
                const dartsCricket1: IDartsCricket = {
                    id: this.props.dartsCricket1.id,
                    score: score1,
                    hits: this.state.hits1,
                    closeNumbers: this.state.closeNumbers1,
                    openNumbers: this.state.openNumbers1,
                }
                if (dartsCricket1.id != undefined) {
                    DartsCricketService.updateDartsCricket(dartsCricket1.id, dartsCricket1).then(()=>{
                        this.props.onChange(this.state.score1, this.state.score2);
                    })
                }
            });
        }
        let idScore2 = this.props.dartsCricket2.score.id;
        if (idScore2 != undefined) {
            ScoreService.getScoreById(idScore2).then((score2: IScore) => {
                score2.value = this.state.score2;
                const dartsCricket2: IDartsCricket = {
                    id: this.props.dartsCricket2.id,
                    score: score2,
                    hits: this.state.hits2,
                    closeNumbers: this.state.closeNumbers2,
                    openNumbers: this.state.openNumbers2,
                }
                if (dartsCricket2.id != undefined) {
                    DartsCricketService.updateDartsCricket(dartsCricket2.id, dartsCricket2).then(()=>{
                        this.props.onChange(this.state.score1, this.state.score2);
                    })
                }
            });
        }
    }
    hit = (val: string) => {

        if (this.state.round % 2 != 0) {
            let hits1: string[] = [];
            hits1 = this.state.hits1;
            var i: number = 0;
            hits1.push(val);
            hits1.forEach(element => {
                if (val == element) {
                    i++;

                }
            });
            if (i == 3) {

                this.state.openNumbers1.push(val);
                this.state.closeNumbers2.push(val);
            }
            if (i > 3) {

                if (!this.checkCloseNumber(val)) {
                    let score1: number = this.state.score1;
                    score1 = score1 + parseInt(val);
                    this.setState({ score1: score1 });
                }
            }
            this.setState({ hits1: hits1 });
            this.update();
        }
        else {
            let hits2: string[] = this.state.hits2;
            var i: number = 0;
            hits2.push(val);
            hits2.forEach(element => {
                if (val == element) {
                    i++;
                }
            });
            if (i == 3) {
                this.state.openNumbers2.push(val);
                this.state.closeNumbers1.push(val);
            }
            if (i > 3) {
                if (!this.checkCloseNumber(val)) {
                    let score2: number = this.state.score2;
                    score2 = score2 + parseInt(val);
                    this.setState({ score2: score2 })
                }
            }
            this.setState({ hits2: hits2 });
            this.update();
        }
    }
    nextRound = () => {
        console.log("NextPlayer");
        let round = this.state.round;
        round++;
        this.setState({ round: round })
    }
    render() {
        if (this.props.typeOfGame == "solo") {
            return (
                <div className="dartsCricket">
                    <span className="score">{`${this.props.dartsCricket1.score.team.players[0].name}-${this.props.dartsCricket1.score.value}`} </span>
                    <span className="score">{`${this.props.dartsCricket2.score.team.players[0].name}-${this.props.dartsCricket2.score.value}`} </span>
                    {this.state.gameStatus!="finish"?(
                    <div className="btnList">
                    <div className="hitsBtn">
                        <button onClick={() => this.hit("20")}>20</button>
                        <button onClick={() => this.hit("19")}>19</button>
                        <button onClick={() => this.hit("18")}>18</button>
                        <button onClick={() => this.hit("17")}>17</button>
                        <button onClick={() => this.hit("16")}>16</button>
                        <button onClick={() => this.hit("15")}>15</button>
                        <button onClick={() => this.hit("25")}>BULL</button>
                    </div>
                    <button className="nextPBtn" onClick={this.nextRound}>Next player</button>
                </div>
                    )
                :null}
                </div>
            );
        }
        else {
            return (
                <div className="dartsCricket">
                    <span className="score">{`${this.props.dartsCricket1.score.team.name}-${this.state.score1}`} </span>
                    <span className="score">{`${this.props.dartsCricket2.score.team.name}-${this.state.score2}`} </span>
                    {this.state.gameStatus!="finish"?(
                        <div className="btnList">
                        <div className="hitsBtn">
                        <button onClick={() => this.hit("20")}>20</button>
                        <button onClick={() => this.hit("19")}>19</button>
                        <button onClick={() => this.hit("18")}>18</button>
                        <button onClick={() => this.hit("17")}>17</button>
                        <button onClick={() => this.hit("16")}>16</button>
                        <button onClick={() => this.hit("15")}>15</button>
                        <button onClick={() => this.hit("25")}>BULL</button>
                        </div>
                        <button className="nextPBtn" onClick={this.nextRound}>Next player</button>
                        </div>
                    )
                    :null}
                </div>
            );
        }
    }

}
