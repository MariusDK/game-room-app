interface IGame
    {
        Id: string;
        Name: string; 
        Type: string;
        Team: ITeam[];
        EmbarrassingMoments: number[];
        VictoryMoments: number[];
        StartOn: Date;
        EndOn: Date;
    }
