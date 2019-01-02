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
    public static getGamesUnfinishOfPlayer = (page:number,playerId:string):Promise<IGame[]> =>
    {
        console.log(playerId);
        return axios
        .get(`https://localhost:44333/api/game?page=${page}&userIdu=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getGamesFinishOfPlayer = (page:number,playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?page=${page}&userIdf=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getGamesUnfinishOfPlayerAndType = (page:number,type:string,playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?page=${page}&type=${type}&userIdu=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getGamesFinishOfPlayerAndType = (page:number,type:string,playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?page=${page}&type=${type}&userIdf=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getUnfinishGamesOrderByStartDate = (page:number,playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?page=${page}&userIduo=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getFinishGamesOrderByEndDate = (page:number,playerId:string):Promise<IGame[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?page=${page}&userIdfo=${playerId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static updateGame = (gameName:string,game:IGame):Promise<string>=>
    {

        return axios
        .put(`https://localhost:44333/api/game?gameName=${gameName}`,game)
        .then((result: AxiosResponse) => result.data)
    }
    public static getPredictionGame = (infoAboutImg:string):Promise<string> =>
    {
        console.log(infoAboutImg);
        return axios
        .get(`https://localhost:44333/api/game?imgAbout=${infoAboutImg}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static deleteGame = (gameId:string):Promise<string> =>
    {
        return axios
        .delete(`https://localhost:44333/api/game?gameId=${gameId}`)
        .then((result: AxiosResponse) => result.data)       
    }
}