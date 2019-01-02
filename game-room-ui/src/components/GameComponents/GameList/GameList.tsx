import * as React from 'react';
import { IGame } from 'src/models/IGame';
import GameService from 'src/services/GameService';
import Game from './GameListForm/Game';
import { ITeam } from 'src/models/ITeam';
import { Redirect } from 'react-router';
import Navigation from 'src/components/Header/Navigation/Navigation';

export interface IGameListState {
    ugames: IGame[];
    fgames: IGame[];
    loading1: boolean;
    loading2: boolean;
    redirect: boolean;
    gameType: string;
}
export default class GameList extends React.Component<any, IGameListState>
{
    constructor(props: any) {
        super(props);
        this.state = {
            ugames: [],
            fgames: [],
            loading1: true,
            loading2: true,
            redirect: false,
            gameType: "solo"
        }
    }
    componentDidMount() {
        let currentUser = localStorage.getItem("currentUser");
        if (currentUser != null) {
            var obj = JSON.parse(currentUser);
            GameService.getGamesUnfinishOfPlayer(1,obj.id).then((result: IGame[]) => {
                this.setState({ ugames: result, loading1: false });
            });
        }
    }
    public selectGame = (game: IGame) => {
        if (!game.id) return;
        var name = game.name;
        localStorage.setItem('currentGame', name);
        var teams: ITeam[] = game.teams;
        teams.forEach(element => {
            if (element.players.length > 1) {
                var gameType = "multi";
                this.setState({ gameType: gameType })
            }
        });
        this.setState({ redirect: true });
        if (game.endOn != null) {
            localStorage.setItem("finishGame", "true");
        }
        else {
            localStorage.setItem("finishGame", "false");
        }
    }
    render() {
        if (this.state.redirect) {
            console.log(this.state.gameType);
            if (this.state.gameType == "solo") {
                return <Redirect to='/gameSoloPage' />
            }
            else {
                return <Redirect to='/gameTeamPage' />
            }
        }
        return (
            <div className="App">
                <Navigation />
                <h1>Unfinish Game List</h1>
                {this.state.loading1 && <h1>Loading</h1>}

                {!this.state.loading1 &&
                    this.state.ugames.map((item, index) => (
                        <Game
                            key={index}
                            game={item}
                            selectGame={this.selectGame}
                            listSelectedGames = {this.state.fgames}
                        >
                        </Game>
                    )
                    )}

                <h1>Finish Game List</h1>
                {this.state.loading2 && <h1>Loading</h1>}

                {!this.state.loading2 &&
                    this.state.fgames.map((item, index) => (
                        <Game
                        key={index}
                        game={item}
                        selectGame={this.selectGame}
                        listSelectedGames = {this.state.fgames}
                    >
                    </Game> 
                    )
                    )}
            </div>
        )
    }


}