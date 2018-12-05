import * as React from 'react';
import GameService from 'src/services/GameService';
import { IGame } from 'src/models/IGame';
import ScoreList from '../SoloGamePage/ScoreList/ScoreList';
import { ISoloGameProps, ISoloGameState } from '../SoloGamePage/SoloGamePage';
import { Redirect } from 'react-router';
import Navigation from '../Header/Navigation/Navigation';

export default class TeamGamePage extends React.Component<ISoloGameProps, ISoloGameState>
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
        console.log("Am intrat");
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
            GameService.finishGame(name, result).then(() => {
                this.setState({ redirect: true });
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
                <Navigation />
                <h1>{this.state.name}</h1>
                <h3>Type of game: {this.state.type}</h3>
                {console.log(1)}
                {!this.state.loading &&
                    <ScoreList
                        gameName={this.state.name}
                        typeOfGame={"multi"}
                        gameType={this.state.type} />
                }
                <button onClick={this.finishGame}>Finish Game</button>
            </div>
        );
    }
}