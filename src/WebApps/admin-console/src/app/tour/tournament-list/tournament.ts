export enum Surface {
    Grass,
    Clay,
    Hard,
    Carpet
}

export enum TournamentLevel {
    GrandSlam,
    Masters1000,
    ATP500,
    ATP250
}

export interface ITournament {
    tournamentId: number;
    title: string,
    level: TournamentLevel,
    surface: Surface,
    mensSingles: boolean,
    womensSingles: boolean,
    mensDoubles: boolean,
    womensDoubles: boolean,
    mixedDoubles: boolean
}