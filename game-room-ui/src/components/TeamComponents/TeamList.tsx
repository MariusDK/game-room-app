import * as React from 'react';
import { RouteComponentProps} from 'react-router';
import Navigation from '../Header/Navigation/Navigation';
import Footer from '../Footer/Footer';
import { ITeam } from 'src/models/ITeam';
import TeamCard from '../TeamCard/TeamCard';
import TeamService from 'src/services/TeamService';
import { IPlayer } from 'src/models/IPlayer';
import "./TeamList.css";

export interface ITeamListState{
    teams: ITeam[];
    loading: boolean;
    blur:boolean;
    userId:string;
}
export interface ITeamListProps extends RouteComponentProps<any> { }
export default class TeamList extends React.Component<ITeamListProps, ITeamListState>
{

    constructor(props: ITeamListProps) {
        super(props);
        this.state = {
            teams: [],
            loading: true,
            blur:false,
            userId:''
        }
    }
    componentDidMount(){
        var playerJson = localStorage.getItem('currentUser');
        var player:IPlayer = JSON.parse(playerJson);
        var userId = player.id;
        this.setState({userId:userId});
        this.getTeamsOfUser(userId);
    }
    getTeamsOfUser = (userId:string) => {
        var teamList:ITeam[]=[];
        TeamService.getTeamsByUserId(userId).then((result:ITeam[])=>{
            if (result.length!=0)
            {
                result.forEach(element => {
                    console.log(element);
                    if (element.name!="")
                    {
                        teamList.push(element);
                    }
                });
            }
            this.setState({teams:teamList,userId:userId});
            console.log(this.state.teams);
        });
    }
    removeTeamFromList = (team:ITeam) => {
       TeamService.deleteTeamById(team.id).then((result:string)=>{
           this.getTeamsOfUser(this.state.userId);
       });   
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
        return (
            <div>
                <Navigation 
                    onAddBlur={this.onAddBlur}
                    onRemoveBlur={this.onRemoveBlur}
                />

            <div className="teamListPage">
                <div className={this.state.blur?"hideTeamListPanel":"TeamListPanel"}>
                <br />
               <div className="teamListCardGame">
                {this.state.teams.map((item, index)=>(
                    <TeamCard team={item}
                              removeTeamFromList={this.removeTeamFromList}
                    />
                )
                )}
                </div>
                </div>
            </div>
            <Footer />
        </div>
        )
    }
}