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
            duplicate: false,
            blur: false
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
    };
    checkDuplicate = (teamCheck:ITeam)=>
    {
        this.setState({duplicate:false});
        const teamsList:ITeam[] = this.state.teams;
        teamsList.forEach(team => {
            if (team.id==teamCheck.id)
            {
                this.setState({duplicate:true});
            }            
        });
    }
    getTeamByName = () => {
        const teamsList: ITeam[] = this.state.teams;
        var name = this.state.teamName;
        this.setState({ error: "" });
        TeamService.getTeamByName(name).then((result: ITeam) => {
            if (result.id != undefined) {
                this.checkDuplicate(result);
                if (this.state.duplicate==false){
                    teamsList.push(result);
                this.setState({ teams: teamsList });
                }
                else{
                    this.setState({ error: "Duplicate team!" });
                }
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
            .then((result: string) => {
                if (result!="Insert Working!")
                {
                    this.setState({ error: result });                       
                }
                else{
                    localStorage.setItem("finishGame", "false"); 
                    this.setState({ redirect: true });
                }
            });
        }
        else {
            console.log("Error")
        }
    }
    removeTeamFromList=(team:ITeam)=>{
        const teamList: ITeam[] = this.state.teams;
        let positionOfElement=-1;
        for (var i=0;i<teamList.length;i++)
        {
            if (teamList[i].id==team.id)
            {
                positionOfElement = i;
                break;
            }
        }
        if (positionOfElement!=-1)
        {
            teamList.splice(positionOfElement,1);
            this.setState({teams:teamList});
        }
    }
    onAddBlur=()=>
    {
        this.setState({blur:true});
    }
    onRemoveBlur=()=>
    {
        this.setState({blur:false});
    }
    public render() {


        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to={`/gameTeamPage/${this.state.name}`} />
        }
        return (<div>
            <div>
                <Navigation 
                    onAddBlur={this.onAddBlur}
                    onRemoveBlur={this.onRemoveBlur}
                />
            </div>
            <div className="multiGameC">
                <div className={this.state.blur?"hideTeamPanel":"createTeamPanel"}>
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
                <button className="searchMultiBtn" onClick={this.getTeamByName}>Search</button>
                <div className="teamCardGame">
                {this.state.teams.map((item, index)=>(
                    <TeamCard team={item}
                              removeTeamFromList={this.removeTeamFromList}
                    />
                )
                )}
                </div>
                <span style={{ color: "red" }}>{this.state.error}</span><br />
                <button className="startBtnMulti" onClick={this.createGame}>Start</button>
                </div>
            </div>

            <Footer />
        </div>
        )
    }
}