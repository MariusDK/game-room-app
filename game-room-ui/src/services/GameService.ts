import axios, {AxiosResponse} from 'axios';
import { IGame } from 'src/models/IGame';

export default class GameService {

    public static insertGame = (game:IGame):Promise<string> =>
    {
        return axios
        .post(`https://localhost:44333/api/game`,game)
        .then((result: AxiosResponse) => result.data)
    }
    public static getGameByName = (gameName:string):Promise<IGame> =>
    {
        console.log(gameName);
        return axios
        .get(`https://localhost:44333/api/game?gameName=${gameName}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static finishGame = (gameName:string,game:IGame):Promise<string> =>
    {
        console.log(gameName);
        return axios
        .put(`https://localhost:44333/api/game?name=${gameName}`,game)
        .then((result: AxiosResponse) => result.data)
    }
    public static getGamesUnfinishOfPlayer = (playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?uplayerId=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getGamesFinishOfPlayer = (playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?fplayerId=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
}