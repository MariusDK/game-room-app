import { ITeam } from './ITeam';

export interface IGame
{
    id?:string;
    name:string;
    type:string;
    location:string;
    referee:string;
    teams:ITeam[];
    startOn?:Date;
    endOn?:Date;
    victoryMoments:string[];
    embarrassingMoments:string[];
    
}