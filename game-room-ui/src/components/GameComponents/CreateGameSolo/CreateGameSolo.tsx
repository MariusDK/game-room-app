import * as React from 'react';
import { RouteComponentProps, Redirect } from 'react-router';
import GameService from 'src/services/GameService';
import TeamService from 'src/services/TeamService';
import GameForm from './GameForm/GameForm';
import PlayerService from 'src/services/PlayerService';
import { IPlayer } from 'src/models/IPlayer';
import { ITeam } from 'src/models/ITeam';
import { IGame } from 'src/models/IGame';
import Navigation from 'src/components/Header/Navigation/Navigation';
import "./CreateGameSolo.css";
import Footer from 'src/components/Footer/Footer';
import PlayerCard from 'src/components/PlayerCard/Suggestions';

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
    error: string;
    duplicate: boolean;
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
            teamName: '',
            error: '',
            duplicate: false
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
    handleDuplicate = (player: IPlayer) => {
        const playerList: IPlayer[] = this.state.players;
        playerList.forEach(element => {
            if (element.id == player.id) {
                this.setState({ duplicate: true });
            }
            else {
                this.setState({ duplicate: false });
            }
        });
    }
    handleValidation = () => {
        let name = this.state.name;
        let players = this.state.players;
        let nameError = '';
        let error = '';


        let formIsValid = true;
        if (!name) {
            formIsValid = false;
            nameError = "Name field cannot be empty";
        }
        if (players.length < 2) {
            formIsValid = false;
            error = "Need more players!"
        }
        this.setState({ nameError: nameError });
        this.setState({ error: error });
        return formIsValid;
    };
    addPlayerToList = () => {
        const teamsList: ITeam[] = this.state.teams;
        const playerList: IPlayer[] = this.state.players;
        PlayerService.getPlayerByUsername(this.state.username).then((player: IPlayer) => {
            if (player.name != null) {
                this.handleDuplicate(player);
                if (!this.state.duplicate) {
                    playerList.push(player);
                    this.setState({ players: playerList })
                    const newTeam: ITeam = {
                        name: '',
                        players: [player],
                    }
                    TeamService.insertTeam(newTeam).then((result: ITeam) => {
                        teamsList.push(result);
                        this.setState({ teams: teamsList });
                    });
                }
                else {
                    this.setState({ error: "Player already exists" });
                }
            }
            else {
                this.setState({ error: "Player don't exist!" });
            }
        });
    }

    createGame = () => {
        if (this.handleValidation()) {
            const newGame: IGame = {
                name: this.state.name,
                type: this.state.type,
                teams: this.state.teams,
                victoryMoments: [],
                embarrassingMoments: []
            }
            localStorage.setItem('currentGame', newGame.name);

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
    removePlayerFromList=(player:IPlayer) =>{
        const playerList: IPlayer[] = this.state.players;
        let positionOfElement=-1;
        for (var i=0;i<playerList.length;i++)
        {
            if (playerList[i].id==player.id)
            {
                positionOfElement = i;
                break;
            }
        }
        if (positionOfElement!=-1)
        {
            console.log(playerList);
            playerList.splice(positionOfElement,1);
            this.setState({players:playerList});
            console.log(positionOfElement);
        }
    }
    public render() {


        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to={`/gameSoloPage/${this.state.name}`} />
        }
        return (<div>
                <Navigation />
                <div className="soloGameC">
                <div className="createSoloPanel">
                <GameForm
                    name={this.state.name}
                    type={this.state.type}
                    nameError={this.state.nameError}
                    handleChange={this.handleChange}

                />
                <br />
                <input className="searchUsername" placeholder="Search using Username..." type="text" name="username" onChange={this.handleChange} />
                <button className="searchBtnSolo" onClick={this.addPlayerToList}>Search</button>
                <div className="playerCardGame">
                {this.state.players.map((item, index) => (
                        <PlayerCard player={item}
                                    removePlayerFromList={this.removePlayerFromList}
                        />
                        )
                    )}
                    </div>
                <span style={{ color: "red" }}>{this.state.error}</span><br />
                <button className="startBtn" onClick={this.createGame}>Start</button>
                </div>
            </div>
            <Footer />
        </div>)
    }





}