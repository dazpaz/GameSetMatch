export enum Surface {
    Grass,
    Clay,
    Hard,
    Carpet
}

export enum TournamentLevel {
    GsmSlam,
    Gsm1000,
    Gsm500,
    Gsm250,
    Gsm125
}

export interface ITournament {
    tournamentId: number;
    title: string,
    location: string,
    level: TournamentLevel,
    surface: Surface,
    mensSingles: boolean,
    womensSingles: boolean,
    mensDoubles: boolean,
    womensDoubles: boolean,
    mixedDoubles: boolean
}