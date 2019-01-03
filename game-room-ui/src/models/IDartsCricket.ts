import { IScore } from './IScore';

export interface IDartsCricket
{
    id?:string;
    score:IScore;
    hits:string[];
    closeNumbers:string[];
    openNumbers:string[];
}