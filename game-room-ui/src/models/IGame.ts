import { ITeam } from './ITeam';

export interface IGame
{
    id?:string;
    name:string;
    type:string;
    teams:ITeam[];
    startOn?:Date;
    endOn?:Date;
}