import * as React from 'react';
import { Redirect } from 'react-router';
import GameService from 'src/services/GameService';
import TeamService from 'src/services/TeamService';
import { ITeam } from 'src/models/ITeam';
import { IGame } from 'src/models/IGame';
import { ICreateGameProps, ICreateGameState } from '../CreateGameSolo/CreateGameSolo';
import GameForm from '../CreateGameSolo/GameForm/GameForm';
import Navigation from 'src/components/Header/Navigation/Navigation';
import "./CreateTeamGame.css";
import Footer from 'src/components/Footer/Footer';
import TeamCard from 'src/components/TeamCard/TeamCard';

export default class CreateTeamGame extends React.Component<ICreateGameProps, ICreateGameState>
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
    handleValidation = () => {
        let name = this.state.name;
        let nameError = '';
        let formIsValid = true;
        let teams = this.state.teams;
        let error = '';
        if (!name) {
            formIsValid = false;
            nameError = "Name field cannot be empty";
        }
        if (teams.length < 2) {
            formIsValid = false;
            error = "Need more teams!"
        }
        this.setState({ nameError: nameError });
        this.setState({ error: error });
        return formIsValid;
        this.setState({ nameError: nameError });
        return formIsValid;
    };

    getTeamByName = () => {
        const teamsList: ITeam[] = this.state.teams;
        var name = this.state.teamName;
        TeamService.getTeamByName(name).then((result: ITeam) => {
            if (result.id != undefined) {
                teamsList.push(result);
                this.setState({ teams: teamsList });
            }
            else {
                this.setState({ error: "Team don't exist!" });
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
    public render() {


        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to='/gameTeamPage' />
        }
        return (<div>

            <div>
                <Navigation />
            </div>
            <div className="multiGameC">
                <div>
                    <GameForm
                        name={this.state.name}
                        type={this.state.type}
                        nameError={this.state.nameError}
                        handleChange={this.handleChange}

                    />
                </div>
                <br />
                <input placeholder="Search using Team Name" type="text" name="teamName" onChange={this.handleChange} />
                <button onClick={this.getTeamByName}>Search</button>

                <TeamCard results={this.state.teams}
                />
                <span style={{ color: "red" }}>{this.state.error}</span><br />
                <button className="startBtnMulti" onClick={this.createGame}>Start</button>
            </div>

            <Footer />
        </div>
        )
    }
}