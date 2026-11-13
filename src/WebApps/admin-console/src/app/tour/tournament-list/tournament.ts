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

export const tournamentLevelLabels: Readonly<Record<TournamentLevel, string>> = {
    [TournamentLevel.GsmSlam]: 'Grand Slam',
    [TournamentLevel.Gsm1000]: 'Masters 1000',
    [TournamentLevel.Gsm500]: 'GSM 500',
    [TournamentLevel.Gsm250]: 'GSM 250',
    [TournamentLevel.Gsm125]: 'GSM 125',
};

export const surfaceLabels: Readonly<Record<Surface, string>> = {
    [Surface.Grass]: 'Grass',
    [Surface.Clay]: 'Clay',
    [Surface.Hard]: 'Hard',
    [Surface.Carpet]: 'Carpet',
};

export interface ITournament {
    tournamentId: string;
    title: string,
    location: string,
    level: TournamentLevel,
    surface: Surface,
    mensSingles: boolean,
    womensSingles: boolean,
    mensDoubles: boolean,
    womensDoubles: boolean,
    mixedDoubles: boolean,
    isActive: boolean
}