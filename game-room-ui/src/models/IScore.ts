import { ITeam } from './ITeam';
import { IGame } from './IGame';

export interface IScore
{
    id?: string;
    team: ITeam;
    game: IGame;
    value: number;
}