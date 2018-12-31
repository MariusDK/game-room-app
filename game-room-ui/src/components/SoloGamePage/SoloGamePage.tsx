import * as React from 'react';
import GameService from 'src/services/GameService';
import { RouteComponentProps, Redirect } from 'react-router';
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
    embarrassingMoments:string[];

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
            victoryMoments:[],
            embarrassingMoments:[]
        }
    }
    onChange=(nameGame:string)=>
    {
        console.log("aici");
        GameService.getGameByName(nameGame).then((result: IGame) => {
                
                
            this.setState({ name: result.name });
            this.setState({ type: result.type });
            if (result.victoryMoments!=undefined){
                this.setState({victoryMoments:result.victoryMoments});
                console.log(result.victoryMoments);
            }
            if (result.embarrassingMoments!=undefined){
            this.setState({embarrassingMoments:result.embarrassingMoments});
            }
            this.setState({ loading: false });
        })
        console.log(this.state.victoryMoments);
    }
    public componentDidMount() {
        let nameGame = localStorage.getItem('currentGame');
        if (nameGame != null) {
            GameService.getGameByName(nameGame).then((result: IGame) => {
                
                
                this.setState({ name: result.name });
                this.setState({ type: result.type });
                if (result.victoryMoments!=undefined){
                    this.setState({victoryMoments:result.victoryMoments});
                    console.log(result.victoryMoments);
                }
                if (result.embarrassingMoments!=undefined){
                this.setState({embarrassingMoments:result.embarrassingMoments});
                }
                this.setState({ loading: false });
            })
        }
    }
    finishGame = () => {
        var name = this.state.name;
        GameService.getGameByName(name).then((result: IGame) => {
            GameService.finishGame(name, result).then(()=>{
                this.setState({redirect:true});
            });
        })
    }
    render() {
        const redirect = this.state.redirect;
        if (redirect) {
            return <Redirect to='/' />
        }
        return (
            <div>
                <Navigation/>
            <div className="soloGamePage">
                <div className="leftGamePage">
                <h1>Game name: {this.state.name}</h1>
                <h3>Type of game: {this.state.type}</h3>
                {console.log(1)}
                {!this.state.loading &&
                    <ScoreList
                        gameName={this.state.name}
                        typeOfGame={"solo"}
                        gameType={this.state.type}
                    />
                }

                <button className="finishGame" onClick={this.finishGame}>Finish Game</button>
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
                    moments = {this.state.victoryMoments}
                />
                
                <h2>Embarrassing Moments</h2>
                <Gallery
                    moments = {this.state.embarrassingMoments}
                />
                </div>
                
            </div>
                <Footer/>
            </div>
        );
    }


}