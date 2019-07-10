import * as React from 'react';
import GameService from 'src/services/GameService';
import { IGame } from 'src/models/IGame';
import ScoreList from '../SoloGamePage/ScoreList/ScoreList';
import { Redirect, RouteComponentProps } from 'react-router';
import Navigation from '../Header/Navigation/Navigation';
import SubmitComponent from '../ImageComponents/SubmitComponent';
import Gallery from '../GalleryComponent/Gallery';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import Footer from '../Footer/Footer';
import './TeamGamePage.css';
import ScoreService from 'src/services/ScoreService';
import { ITeam } from 'src/models/ITeam';

export interface ITeamGameState {
    name: string;
    type: string;
    loading: boolean;
    redirect: boolean;
    victoryMoments: string[];
    embarrassingMoments: string[];
    blur:boolean;
    gameState:boolean;
    open: boolean;
    prediction: string;
    procentualPrediction: number;

}
export interface ITeamGameProps extends RouteComponentProps<any> { }
export default class TeamGamePage extends React.Component<ITeamGameProps, ITeamGameState>
{
    constructor(props: ITeamGameProps) {
        super(props);
        this.state = {
            name: '',
            type: '',
            loading: true,
            redirect: false,
            victoryMoments: [],
            embarrassingMoments: [],
            blur:false,
            gameState: false,
            open: false,   
            prediction: '',
            procentualPrediction: 0  
        }
    }
    onChange = (nameGame: string) => {
        GameService.getGameByName(nameGame).then((result: IGame) => {
            this.setState({ name: result.name });
            this.setState({ type: result.type });
            if (result.victoryMoments != undefined) {
                this.setState({ victoryMoments: result.victoryMoments });
            }
            if (result.embarrassingMoments != undefined) {
                this.setState({ embarrassingMoments: result.embarrassingMoments });
            }
            this.setState({ loading: false });
        })
    }
    public componentDidMount() {
        var gameState = localStorage.getItem('gameState');
        if (gameState=='finish')
        {
            this.setState({gameState:true});
        }
        else{
            this.setState({gameState:false});
        }
        let nameGame = this.props.match.params.gameName;
        //let nameGame = localStorage.getItem('currentGame');
        if (nameGame != null) {
            this.onChange(nameGame);
        }
    }
    handleClickOpen = () => {
        var player = JSON.parse(localStorage.getItem('currentUser'));
        var playerTeam:ITeam;
        console.log(player);
        GameService.getGameByName(this.state.name).then((result: IGame) => {
            var teams = result.teams;
            teams.forEach(team => {
                team.players.forEach(element => {
                    if (player.id==element.id)
                    {
                        playerTeam = team;
                    }
                });
            });
            ScoreService.getPredictionTeam(result.id,playerTeam.id).then((prediction:string)=>{
                console.log(prediction);
                this.setState({ open: true, prediction: prediction });
            })
        }); 
        GameService.getGameByName(this.state.name).then((result: IGame) => {
            ScoreService.getProcentualPredictionTeam(result.id,playerTeam.id).then((procentualPrediction:number)=>{
                this.setState({ open: true, procentualPrediction: procentualPrediction });
            })
        }); 
    }
    handleClose = () => {
        this.setState({ open: false });
    }
    finishGame = () => {
        var name = this.state.name;
        GameService.getGameByName(name).then((result: IGame) => {
            GameService.finishGame(name, result).then(() => {
                this.setState({ redirect: true });
            });
        })
    }
    onAddBlur=()=>
    {
        this.setState({blur:true});
    }
    onRemoveBlur=()=>{
        this.setState({blur:false});
    }
    render() {
        const redirect = this.state.redirect;
        if (redirect) {
            return <Redirect to='/' />
        }
        return (
            <div>
                <Navigation 
                    onAddBlur={this.onAddBlur}
                    onRemoveBlur={this.onRemoveBlur}
                />
                <div className="teamGamePage">
                <div className={this.state.blur?"hideTeamGamePanel":"teamGamePanel"}>
                <h1>Game name: {this.state.name}</h1>
                        <h3>Type of game: {this.state.type}</h3>
                    <div className="centerPart">
                    <div className="leftGamePage">
                    <div>
                                <button className="predictGameBtn" onClick={this.handleClickOpen}>Click here to find if you win</button>
                                </div>
                                <Dialog id="predictionDialog"
                                    open={this.state.open}
                                    color="#503C2C"
                                >
                                    <DialogTitle id="alert-dialog-title">{"Prediction"}</DialogTitle>
                                    <DialogContent id="alert-dialog-content">
                                        <DialogContentText id="alert-dialog-description">
                                        Classification - Prediction here: {this.state.prediction} <br></br>
                                        Regression - The chance to win: {this.state.procentualPrediction}%
                                        </DialogContentText>
                                    </DialogContent>
                                    <DialogActions>
                                        <button className="finishGameBtn" onClick={this.handleClose} color="primary">
                                            Thank You
                                        </button>
                                    </DialogActions>
                                </Dialog>
                        {!this.state.loading &&
                            <ScoreList
                                gameName={this.state.name}
                                typeOfGame={"multi"}
                                gameType={this.state.type} />
                        }
                    </div>
                    <div className="rightGamePage">
                        <div className="submitImage">
                            <SubmitComponent
                                gameName={this.state.name}
                                onChange={this.onChange}
                            />
                        </div>

                        <h2>Victory Moments</h2>
                        <Gallery
                            moments={this.state.victoryMoments}
                            listType="vicMoments"
                            gameName={this.state.name}
                        />

                        <h2>Embarrassing Moments</h2>
                        <Gallery
                            moments={this.state.embarrassingMoments}
                            listType="embMoments"
                            gameName={this.state.name}
                        />
                        </div>
                    </div>
                    <div className={this.state.gameState?"hideFinishButton":"finisButtonZone"}>
                    <button className="finishGameBtn" onClick={this.finishGame}>Finish Game</button>
                    </div>
                    </div>
                </div>
                <Footer />
            </div>
        );
    }
}