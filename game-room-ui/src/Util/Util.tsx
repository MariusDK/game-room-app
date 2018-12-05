import { IDartsX01 } from 'src/models/IDartsX01';

export default class Util
{
    public static leaderboardDartsX01=(dartsX01s:IDartsX01[]):IDartsX01[] =>
    {
        let dartsX01List:IDartsX01[] = dartsX01s;
        for (let i = 0; i < dartsX01List.length - 1; i++)
            {
                for (let j = i+1; j < dartsX01List.length; j++)
                {
                    if (dartsX01List[i].stateScore > dartsX01List[j].stateScore)
                    {
                        var aux = dartsX01List[i];
                        dartsX01List[i] = dartsX01List[j];
                        dartsX01List[j] = aux;
                    }
                }
            }
            return dartsX01List;
    }
}