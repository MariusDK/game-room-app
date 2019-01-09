import * as React from 'react';
import { IPlayer } from 'src/models/IPlayer';
import { RouteComponentProps, Redirect } from 'react-router';
import { ITeam } from 'src/models/ITeam';
import TeamService from 'src/services/TeamService';
import TeamForm from './TeamForm/TeamForm';
import PlayerService from 'src/services/PlayerService';
import Navigation from '../Header/Navigation/Navigation';
import Footer from '../Footer/Footer';
import './CreateTeam.css';
import Suggestions from '../PlayerCard/Suggestions';

export interface ITeamState {
    name: string;
    players: IPlayer[];
    loading: boolean;
    redirect: boolean;
    nameError: string;
    username: string;
    errorMessage: string;
    playerExist: boolean;
    blur: boolean;
}
export interface ITeamProps extends RouteComponentProps<any> { }

export default class CreateTeam extends React.Component<ITeamProps, ITeamState>
{
    constructor(props: ITeamProps) {
        super(props);
        this.state = {
            name: '',
            players: [],
            loading: false,
            redirect: false,
            nameError: '',
            username: '',
            errorMessage: '',
            playerExist: false,
            blur:false
        }
    }

    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: ITeamState) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }

    handleValidation() {
        let name = this.state.name;
        let nameError = '';
        let formIsValid = true;
        if (!name) {
            formIsValid = false;
            nameError = "Name field cannot be empty";
        }
        this.setState({ nameError: nameError });
        return formIsValid;
    }
    checkIfPlayerExist = (searchPlayer:IPlayer) =>
    {
        this.setState({playerExist:false});
        this.state.players.forEach( player=> {
            if (searchPlayer.id==player.id)
            {
                this.setState({playerExist:true});
            }            
        });
    }
    addPlayerToList = () => {
        const playerList: IPlayer[] = this.state.players;
        PlayerService.getPlayerByUsername(this.state.username).then((player: IPlayer) => {
            if (player.name != null) {
                this.checkIfPlayerExist(player);
                if (this.state.playerExist==false)
                {
                    playerList.push(player);
                this.setState({ players: playerList });
                this.setState({ errorMessage: "" });
                }
                else{
                    this.setState({ errorMessage: "Player was selected!" });
                }
            }
            else {
                this.setState({ errorMessage: "Player not found!" });
            }
        });
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
    addTeam = () => {
        this.setState({ loading: true });
        if (this.handleValidation()) {
            const newTeam: ITeam = {
                name: this.state.name,
                players: this.state.players
            }
            TeamService.insertTeam(newTeam)
                .then(() => {
                    this.props.history.push('/')
                });
            this.setState({ redirect: true });
        }
        else {
            this.setState({ errorMessage: "Creation Failed!" });
        }
        this.setState({ loading: false })
    }
    onAddBlur=()=>
    {
        this.setState({blur:true});
    }
    onRemoveBlur=()=>{
        this.setState({blur:false});
    }
    public render() {

        const { redirect } = this.state;
        if (redirect) {
            <Redirect to='/' />
        }
        return (
            <div>
                <Navigation 
                    onAddBlur={this.onAddBlur}
                    onRemoveBlur={this.onRemoveBlur}
                />
                <div className="createTeam">
                <div className={this.state.blur?"hideTeamPanel":"teamPanel"}>
                    <div className="teamForm">
                        <TeamForm
                            name={this.state.name}
                            nameError={this.state.nameError}
                            handleChange={this.handleChange}
                        />
                    </div>
                    <div>
                        <input className="searchInput" placeholder="Search for..." type="text" name="username" onChange={this.handleChange} />
                        <button className="searchBtn" onClick={this.addPlayerToList}>Search</button><br></br>
                    </div>
                    <span style={{ color: "red" }}>{this.state.errorMessage}</span><br />
                    <div className="playerCard">
                    {this.state.players.map((item, index) => (
                        <Suggestions player={item}
                                    removePlayerFromList={this.removePlayerFromList}
                        />
                        )
                    )}
                    </div>
                    <button className="saveBtn" onClick={this.addTeam}>Save</button>
                    </div>
                </div>
                <Footer />
            </div>
        )
    }

}
