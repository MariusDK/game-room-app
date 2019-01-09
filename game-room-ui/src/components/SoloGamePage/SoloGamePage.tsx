import * as React from 'react';
import GameService from 'src/services/GameService';
import { RouteComponentProps, Redirect} from 'react-router';
import ScoreList from './ScoreList/ScoreList';
import { IGame } from 'src/models/IGame';
import Navigation from '../Header/Navigation/Navigation';
import Footer from '../Footer/Footer';
import './SoloGamePage.css';
import SubmitComponent from '../ImageComponents/SubmitComponent';
import Gallery from '../GalleryComponent/Gallery';

export interface ISoloGameState {
    name: string;
    type: string;
    loading: boolean;
    redirect: boolean;
    victoryMoments: string[];
    embarrassingMoments: string[];
    blur:boolean;
    gameState:boolean;

}
export interface ISoloGameProps extends RouteComponentProps<any> { }
export default class SoloGamePage extends React.Component<ISoloGameProps, ISoloGameState>
{
    constructor(props: ISoloGameProps) {
        super(props);
        this.state = {
            name: '',
            type: '',
            loading: true,
            redirect: false,
            victoryMoments: [],
            embarrassingMoments: [],
            blur:false,
            gameState:false
        }
    }
    onChange = (nameGame: string) => {
        
        GameService.getGameByName(nameGame).then((result: IGame) => {
            this.setState({ name: result.name });
            this.setState({ type: result.type });
            if (result.victoryMoments != undefined) {
                this.setState({ victoryMoments: result.victoryMoments });
                console.log(this.state.victoryMoments);
            }
            if (result.embarrassingMoments != undefined) {
                this.setState({ embarrassingMoments: result.embarrassingMoments });
            }
            
            this.setState({ loading: false });
        })
    }
    public componentDidMount() {
        let nameGame = this.props.match.params.gameName;
        //let nameGame = localStorage.getItem('currentGame');
        let gameState = localStorage.getItem('gameState');
        if (gameState=='finish')
        {
            this.setState({gameState:true});
        }
        else{
            this.setState({gameState:false});
        }
        if (nameGame != null) {
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
            return <Redirect to='/unfinishGames' />
        }
        return (
            
            <div>
                <Navigation 
                    onAddBlur={this.onAddBlur}
                    onRemoveBlur={this.onRemoveBlur}
                />
                <div className="soloGamePage">
                    <div className={this.state.blur?"hideSoloGamePanel":"soloGamePanel"}>
                    <div className="upperPart">
                    <h1>Game name: {this.state.name}</h1>
                        <h3>Type of game: {this.state.type}</h3>
                    </div>
                    <div className="centerPart">
                    <div className="leftGamePage">
                        {!this.state.loading &&
                            <ScoreList
                                gameName={this.state.name}
                                typeOfGame={"solo"}
                                gameType={this.state.type}
                            />
                        }

                       
                    </div>
                    <div className="rightGamePage">
                    <div className="submitImageComp">
                            <SubmitComponent
                                gameName={this.state.name}
                                onChange={this.onChange}
                            />
                        </div>
                        <h5>Click the image for AI functionality!</h5>

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