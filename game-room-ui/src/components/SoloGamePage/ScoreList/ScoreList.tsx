import * as React from 'react';
import { IScore } from 'src/models/IScore';
import ScoreService from 'src/services/ScoreService';
import Score from '../Score/Score';
import DartsX01Score from '../DartsX01Score/DartsX01Score';
import { IDartsX01 } from 'src/models/IDartsX01';
import DartsX01Service from 'src/services/DartsX01Service';
import DartsCricketScore from '../DartsCricketScore/DartsCricketScore';
import { IDartsCricket } from 'src/models/IDartsCricket';
import DartsCricketService from 'src/services/DartsCricketService';
import Leaderboard from 'src/components/Leaderboard/Leaderboard';
import GameService from 'src/services/GameService';
import { IGame } from 'src/models/IGame';
import Util from 'src/Util/Util';
import "./ScoreList.css";


export interface IScoreListProps {
    gameName: string;
    typeOfGame: string;
    gameType: string;
}
export interface IScoreListState {
    scores: IScore[];
    leaderboard: IScore[];
    dartsX01: IDartsX01[];
    dartsCricket: IDartsCricket[];
    loading: boolean;
    loading2: boolean;
    leaderboardDartsX01: IDartsX01[];
    finishGame: string;
    refresh: boolean;
}
export default class ScoreList extends React.Component<IScoreListProps, IScoreListState>
{
    constructor(props: IScoreListProps) {
        super(props);
        this.state = {
            scores: [],
            dartsX01: [],
            dartsCricket: [],
            loading: true,
            leaderboard: [],
            loading2: true,
            leaderboardDartsX01: [],
            finishGame: '',
            refresh: true
        }
    }
    public componentDidMount() {
        this.getData();
    }
    getData = () => {
        this.setState({ dartsX01: [] });
        this.setState({ dartsCricket: [] });
        let finishGame = localStorage.getItem("finishGame")
        if (finishGame != null) {
            this.setState({ finishGame: finishGame });
        }
        var dartsX01List: IDartsX01[] = this.state.dartsX01;
        var dartsCricketList: IDartsCricket[] = this.state.dartsCricket;
        ScoreService.getScoresOfGame(this.props.gameName).then((result: IScore[]) => {

            if (this.props.gameType == "Darts/X01") {
                result.forEach(element => {
                    if (element.id != undefined) {
                        DartsX01Service.getDartsX01ByScore(element.id).then((dartsX01: IDartsX01) => {
                            dartsX01List.push(dartsX01);
                            if (dartsX01List.length == result.length) {
                                let dartsX01Leaderboard = Util.leaderboardDartsX01(dartsX01List);
                                this.setState({ dartsX01: dartsX01List });
                                this.setState({ scores: result, loading: false });
                                this.setState({ leaderboardDartsX01: dartsX01Leaderboard, loading2: false });
                            }
                        })
                    }
                });

            }
            else if (this.props.gameType == "Darts/Cricket") {
                result.forEach(element => {

                    if (element.id != undefined) {

                        DartsCricketService.getDartsCricketByScore(element.id).then((dartsCricket: IDartsCricket) => {
                            dartsCricketList.push(dartsCricket);
                            this.setState({ dartsCricket: dartsCricketList });
                            if (dartsCricketList.length == result.length) {
                                this.setState({ scores: result, loading: false });

                            }

                        })
                    }
                });

            }
            else {
                this.setState({ scores: result, loading: false });
            }
        });
        GameService.getGameByName(this.props.gameName).then((result: IGame) => {
            if (result.id != undefined) {
                ScoreService.getLeaderboard(result.id).then((listScore: IScore[]) => {
                    this.setState({ leaderboard: listScore, loading2: false })
                })
            }
        });
    }
    start301 = () => {
        const dartsX01List:IDartsX01[]=this.state.dartsX01;

        dartsX01List.forEach(element => {
            element.stateScore = 301;
            if (element.id != undefined) {
                DartsX01Service.updateDartsX01(element.id, element).then(() => {
                    this.setState({ refresh: true });
                });
            }
        });
        console.log("aici");
        this.setState({dartsX01:dartsX01List});
    }
    start501 = () => {
        const dartsX01List:IDartsX01[]=this.state.dartsX01;
        dartsX01List.forEach(element => {
            element.stateScore = 501;
            if (element.id != undefined) {
                DartsX01Service.updateDartsX01(element.id, element).then(() => {
                    this.setState({ refresh: true });
                });
            }
        });
        console.log("aici");
        this.setState({dartsX01:dartsX01List});
    }
    start1001 = () => {
        const dartsX01List:IDartsX01[]=this.state.dartsX01;
        dartsX01List.forEach(element => {
            element.stateScore = 1001;
            if (element.id != undefined) {
                DartsX01Service.updateDartsX01(element.id, element).then(() => {
                    this.setState({ refresh: true });
                });
            }
        });
        console.log("aici");
        this.setState({dartsX01:dartsX01List});
    }
    onChange = (score:IScore) => {
        const scoreList:IScore[] = this.state.scores;
        scoreList.forEach(element => {
            if (score.id==element.id)
            {
                element.value = score.value;
            }            
        });
        this.setState({scores:scoreList});
        this.getData();
    }
    onChangeDartsX01 = () => {
        this.getData();
    }
    onChangeDartsCriket = (score1: number, score2: number) => {
        var dartsCricket1 = this.state.dartsCricket[0];
        var dartsCricket2 = this.state.dartsCricket[1];
        dartsCricket1.score.value = score1;
        dartsCricket2.score.value = score2;
        var dartsCricket: IDartsCricket[] = [];
        dartsCricket.push(dartsCricket1);
        dartsCricket.push(dartsCricket2);
        this.setState({ dartsCricket: dartsCricket });
    }
    public render() {
        if (this.props.gameType == "Darts/X01") {
            return (
                <div>
                    <p>Choose starting score:</p>
                    <div className="startPoint">
                    <button onClick={this.start301}>301</button>
                    <button onClick={this.start501}>501</button>
                    <button onClick={this.start1001}>1001</button>
                    </div>
                    {(this.props.typeOfGame == "multi") &&
                        <h2>Team List</h2>}
                    {(this.props.typeOfGame == "solo") &&
                        <h2>Players List</h2>}
                    {!this.state.loading && this.state.finishGame != "true" &&
                        this.state.dartsX01.map((item, index) => (
                            <DartsX01Score
                                key={index}
                                dartsX01={item}
                                score={item.score}
                                typeOfGame={this.props.typeOfGame}
                                scoreValue={item.stateScore}
                                onChangeDartsX01={this.onChangeDartsX01}
                            >
                            </DartsX01Score>
                        )

                        )}
                    <h2>Leaderboard</h2>
                    {!this.state.loading2 &&
                        this.state.leaderboardDartsX01.map((item, index) => (
                            <Leaderboard
                                key={index}
                                position={index}
                                score={item.score}
                                typeOfGame={this.props.typeOfGame}
                                scoreValue={item.stateScore}
                            >
                            </Leaderboard>

                        )
                        )}
                </div>
            );
        }
        if (((this.props.gameType == "Darts/Cricket") && (!this.state.loading)) && (this.state.finishGame != "true")) {
            return (
                <div>
                    <h2>Players List</h2>
                    <DartsCricketScore
                        typeOfGame={this.props.typeOfGame}
                        dartsCricket1={this.state.dartsCricket[0]}
                        dartsCricket2={this.state.dartsCricket[1]}
                        onChange={this.onChangeDartsCriket}
                    >
                    </DartsCricketScore>
                </div>
            );
        }
        else {
            return (
                <div className="scoreList">
                <div className="scorePanel">
                    {(this.props.typeOfGame == "multi") &&
                        <h2>Team List</h2>}
                    {(this.props.typeOfGame == "solo") &&
                        <h2>Players List</h2>}
                    {!this.state.loading && this.state.finishGame != "true" &&
                        this.state.scores.map((item, index) => (

                            <Score
                                key={index}
                                score={item}
                                typeOfGame={this.props.typeOfGame}
                                scoreValue={item.value}
                                onChange={this.onChange}
                            >
                            </Score>
                        )
                        )}
                </div>
                <div className="leaderboardPanel">
                    <h2>Leaderboard</h2>
                    {!this.state.loading2 &&
                        this.state.leaderboard.map((item, index) => (                          
                            <Leaderboard
                                key={index}
                                position={index}
                                score={item}
                                typeOfGame={this.props.typeOfGame}
                                scoreValue={item.value}
                            >
                            </Leaderboard>
                        )
                        )}
                    </div>
                </div>
            );
        }
    }

}