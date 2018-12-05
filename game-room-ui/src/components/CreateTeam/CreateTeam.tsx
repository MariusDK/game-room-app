import * as React from 'react';
import { IPlayer } from 'src/models/IPlayer';
import { RouteComponentProps, Redirect } from 'react-router';
import { ITeam } from 'src/models/ITeam';
import TeamService from 'src/services/TeamService';
import TeamForm from './TeamForm/TeamForm';
import PlayerService from 'src/services/PlayerService';

export interface ITeamState{
    name:string;
    players:IPlayer[];
    loading: boolean;
    redirect: boolean;
    nameError: string;
    username: string;
}
export interface ITeamProps extends RouteComponentProps<any>{}

export default class CreateTeam extends React.Component<ITeamProps,ITeamState>
{
    constructor(props:ITeamProps)
    {
        super(props);
        this.state = {
            name:'',
            players:[],
            loading:false,
            redirect:false,
            nameError:'',
            username:''
        }
    }

    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: ITeamState)=>(
        {
            ...prevState,
            [name]:value
        }
        ));
    }

    handleValidation(){
        let name = this.state.name;
        let nameError = '';
        let formIsValid = true;
        if (!name)
        {
            formIsValid = false;
            nameError = "Name field cannot be empty";
        }
        this.setState({nameError:nameError});
        return formIsValid;
    }
    addPlayerToList=()=>
    {
        const playerList:IPlayer[] = this.state.players;
        PlayerService.getPlayerByUsername(this.state.username).then((player:IPlayer)=>{
            if (player.name!=null)
            {
                console.log(player);
                playerList.push(player);
                this.setState({players:playerList});
            }
        });
    }
    
    addTeam = () => {
        this.setState({loading:true});
        if (this.handleValidation())
        {
            const newTeam: ITeam = {
                name: this.state.name,
                players: this.state.players
            }
            TeamService.insertTeam(newTeam)
                .then(()=>{
                    this.props.history.push('/')
                });
                this.setState({redirect:true});
        }
        else{
            console.log("Error");
        }
        this.setState({loading:false})
    }
    public render(){
    
        const {redirect} = this.state;
        if (redirect)
        {
            <Redirect to='/'/>
        }
    return (
    <div className="App">
            <TeamForm
                name = {this.state.name}
                nameError = {this.state.nameError}
                handleChange = {this.handleChange}
            />
            <input placeholder="Search for..." type="text" name="username" onChange={this.handleChange}/>
            <button onClick={this.addPlayerToList}>Search</button><br></br>
            <button onClick={this.addTeam}>Save</button>
        </div>
    )
    }

}
