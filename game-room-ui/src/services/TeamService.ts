import axios, {AxiosResponse} from 'axios';
import { ITeam } from 'src/models/ITeam';

export default class TeamService {

    public static insertTeam = (team:ITeam):Promise<ITeam> =>
    {
        return axios
        .post(`https://localhost:44333/api/team`,team)
        .then((result: AxiosResponse) => result.data)
    }
    public static getTeamByName = (teamName:string):Promise<ITeam> =>
    {
        return axios
        .get(`https://localhost:44333/api/team?teamName=${teamName}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getTeamsByUserId = (idPlayer:string):Promise<ITeam[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/team?idPlayer=${idPlayer}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static deleteTeamById = (idTeam:string):Promise<string> =>
    {
        return axios
        .delete(`https://localhost:44333/api/team?teamId=${idTeam}`)
        .then((result: AxiosResponse) => result.data)
    }
    

}