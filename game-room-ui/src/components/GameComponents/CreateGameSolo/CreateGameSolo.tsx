import * as React from 'react';
import { RouteComponentProps, Redirect } from 'react-router';
import GameService from 'src/services/GameService';
import TeamService from 'src/services/TeamService';
import GameForm from './GameForm/GameForm';
import PlayerService from 'src/services/PlayerService';
import { IPlayer } from 'src/models/IPlayer';
import { ITeam } from 'src/models/ITeam';
import { IGame } from 'src/models/IGame';
import Suggestions from 'src/components/PlayerCard/Suggestions';
import Navigation from 'src/components/Header/Navigation/Navigation';
import "./CreateGameSolo.css";

export interface ICreateGameState {
    name: string;
    type: string;
    players: IPlayer[];
    teams: ITeam[];
    nameError: string;
    loading: boolean;
    size: number;
    redirect: boolean;
    insertError: string;
    username: string;
    search: boolean;
    teamName: string;
}
export interface ICreateGameProps extends RouteComponentProps<any> { }
export default class CreateGameSolo extends React.Component<ICreateGameProps, ICreateGameState>
{

    constructor(props: ICreateGameProps) {
        super(props);
        this.state = {
            size: 0,
            name: '',
            type: '',
            players: [],
            teams: [],
            nameError: '',
            loading: true,
            redirect: false,
            insertError: '',
            username: '',
            search: false,
            teamName: ''
        }
    }
    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: any) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }
    handleValidation = () => {
        let name = this.state.name;
        let nameError = '';
        let formIsValid = true;
        if (!name) {
            formIsValid = false;
            nameError = "Name field cannot be empty";
        }
        this.setState({ nameError: nameError });
        return formIsValid;
    };
    addPlayerToList = () => {
        const teamsList: ITeam[] = this.state.teams;
        const playerList: IPlayer[] = this.state.players;
        PlayerService.getPlayerByUsername(this.state.username).then((player: IPlayer) => {
            if (player.name != null) {
                console.log(player);
                playerList.push(player);
                this.setState({ players: playerList })
                const newTeam: ITeam = {
                    name: '',
                    players: [player],
                }
                console.log(newTeam);
                TeamService.insertTeam(newTeam).then((result: ITeam) => {
                    teamsList.push(result);
                    this.setState({ teams: teamsList });
                    console.log(this.state.teams);
                });
            }
        });
    }

    createGame = () => {
        if (this.handleValidation()) {
            const newGame: IGame = {
                name: this.state.name,
                type: this.state.type,
                teams: this.state.teams,
            }
            localStorage.setItem('currentGame', newGame.name);
            console.log(newGame);

            GameService.insertGame(newGame)
                .then((insertErrorM: string) => {
                    this.setState({ insertError: insertErrorM });
                    localStorage.setItem("finishGame", "false");
                    this.setState({ redirect: true });
                });
        }
        else {
            console.log("Error")
        }
    }
    public render() {


        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to='/gameSoloPage' />
        }
        return (
            <div className="soloGameC">
                <Navigation />
                <GameForm
                    name={this.state.name}
                    type={this.state.type}
                    nameError={this.state.nameError}
                    handleChange={this.handleChange}

                />
                <br/>
                <input placeholder="Search using Username..." type="text" name="username" onChange={this.handleChange} />
                <button onClick={this.addPlayerToList}>Search</button>

                <Suggestions results={this.state.players}
                />
                <button onClick={this.createGame}>Start</button>
            </div>)
    }





}