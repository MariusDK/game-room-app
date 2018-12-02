import axios, {AxiosResponse} from 'axios';


export default class PlayerService {
    
    public static insertPlayer = (player: IPlayer): Promise<IPlayer> => {
        return axios
        .post(`https://localhost:44333/api/player`,player)
        .then((result: AxiosResponse) => 
        result.data
        )
    }
    public static login = (username:string,password:string): Promise<IPlayer>=> {
        var payload = {
            "username": username,
            "password": password
        }
        return axios
        .post(`https://localhost:44333/api/player/login`,payload)
        .then((result: AxiosResponse) =>
        result.data
        ).catch (error => console.log(error));
        
    }
    public static getPlayerById = (playerId: string): Promise<IPlayer> =>
    {
        return axios
            .get(`https://localhost:44333/api/player?playerId=${playerId}`)
            .then((result: AxiosResponse)=>result.data);
    }
    public static updatePlayer = (playerId: string, player: IPlayer): Promise<String> =>
    {
        return axios
        .put(`https://localhost:44333/api/player?playerId=${playerId}`,player)
        .then((result: AxiosResponse)=>result.data)
    }
}