import axios, {AxiosResponse} from 'axios';
import { IScore } from 'src/models/IScore';

export default class ScoreService {

    public static getScoresOfGame = (gameName:string):Promise<IScore[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/score?gameName=${gameName}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getLeaderboard = (gameId:string):Promise<IScore[]> =>
    {
        return axios
        .get(`https://localhost:44333/api/game?gameIdl=${gameId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getScoreById = (scoreId:string):Promise<IScore> =>
    {
        return axios
        .get(`https://localhost:44333/api/score?scoreId=${scoreId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getPrediction = (gameId:string,playerId:string):Promise<string>=>{
        return axios
        .get(`https://localhost:44333/api/score?gameId=${gameId}&playerId=${playerId}`)
        .then((result:AxiosResponse) => result.data)
    }
    public static getPredictionTeam = (gameId:string,teamId:string):Promise<string>=>{
        return axios
        .get(`https://localhost:44333/api/score?gameId=${gameId}&teamId=${teamId}`)
        .then((result:AxiosResponse) => result.data)
    }
    public static getProcentualPrediction = (gameId:string,playerId:string):Promise<number>=>{
        return axios
        .get(`https://localhost:44333/api/score?gameId=${gameId}&playerId=${playerId}&regression=${'regression'}`)
        .then((result:AxiosResponse) => result.data)
    }
    public static getProcentualPredictionTeam = (gameId:string,teamId:string):Promise<number>=>{
        return axios
        .get(`https://localhost:44333/api/score?gameId=${gameId}&teamId=${teamId}&regression=${'regression'}`)
        .then((result:AxiosResponse) => result.data)
    }
    public static updateScore = (scoreId:string,score:IScore):Promise<string> =>
    {
        return axios
        .put(`https://localhost:44333/api/score?scoreId=${scoreId}`,score)
        .then((result: AxiosResponse) => result.data)
    }
}