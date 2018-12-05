import { IScore } from './IScore';

export interface IDartsCricket
{
    id?:string;
    score:IScore;
    Hits:string[];
    closeNumbers:string[];
    openNumbers:string[];
}