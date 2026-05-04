export enum Surface {
    Grass,
    Clay,
    Hard,
    Carpet
}

export enum TournamentLevel {
    GrandSlam,
    TourFinals,
    Masters1000,
    ATP500,
    ATP250
}

export interface ITournament {
    tournamentId: string,
    title: string,
    level: TournamentLevel,
    surface: Surface,
    venue: string,
    country: string,
    mensSingles: boolean,
    womensSingles: boolean,
    mensDoubles: boolean,
    womensDoubles: boolean,
    mixedDoubles: boolean
}