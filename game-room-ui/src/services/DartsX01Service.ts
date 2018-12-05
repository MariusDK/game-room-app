import axios, {AxiosResponse} from 'axios';
import { IDartsX01 } from 'src/models/IDartsX01';

export default class DartsX01Service 
{
    public static getDartsX01ById=(dartsId:string):Promise<IDartsX01> =>
    {
        console.log(dartsId);
        return axios
        .get(`https://localhost:44333/api/dartsX01?dartsId=${dartsId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getDartsX01ByScore=(scoreId:string):Promise<IDartsX01> =>
    {
        return axios
        .get(`https://localhost:44333/api/dartsX01?scoreId=${scoreId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static updateDartsX01 = (dartsId:string,dartsX01:IDartsX01):Promise<string> =>
    {
        return axios
        .put(`https://localhost:44333/api/dartsX01?dartsId=${dartsId}`,dartsX01)
        .then((result: AxiosResponse) => result.data)
    }


}