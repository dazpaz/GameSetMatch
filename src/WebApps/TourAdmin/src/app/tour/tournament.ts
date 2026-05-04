export enum Surface {
    Grass,
    Clay,
    Hard,
    Carpet
}

export enum TournamentLevel {
    GrandSlam,
    TourFinals,
    GSM1000,
    GSM500,
    GSM250
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