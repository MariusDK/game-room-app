import { IPlayer } from './IPlayer';

export interface ITeam
{
    id?:string;
    name:string;
    players:IPlayer[];
}