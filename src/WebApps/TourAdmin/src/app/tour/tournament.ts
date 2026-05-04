export enum Surface {
    Grass = "Grass",
    Clay = "Clay",
    Hard = "Hard",
    Carpet = "Carpet"
}

export enum TournamentLevel {
    GrandSlam = "Grand Slam",
    TourFinals = "Tour Finals",
    GSM1000 = "Masters 1000",
    GSM500 = "GSM 500",
    GSM250 = "GSM 250"
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