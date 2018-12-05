import * as React from 'react';
import GameService from 'src/services/GameService';
import { RouteComponentProps, Redirect } from 'react-router';
import ScoreList from './ScoreList/ScoreList';
import { IGame } from 'src/models/IGame';
import Navigation from '../Header/Navigation/Navigation';

export interface ISoloGameState {
    name: string;
    type: string;
    loading: boolean;
    redirect: boolean;
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
        }
    }
    public componentDidMount() {
        let nameGame = localStorage.getItem('currentGame');
        if (nameGame != null) {
            GameService.getGameByName(nameGame).then((result: IGame) => {
                console.log(result.id);
                this.setState({ name: result.name });
                this.setState({ type: result.type });
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
            <div className="App">
            <Navigation/>
                <h1>{this.state.name}</h1>
                <h3>Type of game: {this.state.type}</h3>
                {console.log(1)}
                {!this.state.loading &&
                    <ScoreList
                        gameName={this.state.name}
                        typeOfGame={"solo"}
                        gameType={this.state.type}
                    />
                }
                <button onClick={this.finishGame}>Finish Game</button>
            </div>
        );
    }


}