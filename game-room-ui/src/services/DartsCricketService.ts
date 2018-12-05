import axios, {AxiosResponse} from 'axios';
import { IDartsCricket } from 'src/models/IDartsCricket';

export default class DartsCricketService 
{
    public static getDartsCricketById=(dartsId:string):Promise<IDartsCricket> =>
    {
        console.log(dartsId);
        return axios
        .get(`https://localhost:44333/api/dartsCricket?dartsId=${dartsId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static getDartsCricketByScore=(scoreId:string):Promise<IDartsCricket> =>
    {
        console.log(scoreId);
        return axios
        .get(`https://localhost:44333/api/dartsCricket?scoreId=${scoreId}`)
        .then((result: AxiosResponse) => result.data)
    }
    public static updateDartsCricket = (dartsId:string,dartsX01:IDartsCricket):Promise<string> =>
    {
        return axios
        .put(`https://localhost:44333/api/dartsCricket?dartsId=${dartsId}`,dartsX01)
        .then((result: AxiosResponse) => result.data)
    }


}