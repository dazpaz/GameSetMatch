import { ITournament, Surface, TournamentLevel } from "./tournament";

export class TournamentData {
  static tournaments: ITournament[] = [
    {
      tournamentId: '1',
      title: 'Australian Open',
      level: TournamentLevel.GrandSlam,
      surface: Surface.Hard,
      venue: 'Melbourne Park',
      country: 'Australia',
      mensSingles: true,
      womensSingles: true,
      mensDoubles: true,
      womensDoubles: true,
      mixedDoubles: true
    },
    {
      tournamentId: '2',
      title: 'French Open',
      level: TournamentLevel.GrandSlam,
      surface: Surface.Clay,
      venue: 'Stade Roland Garros',
      country: 'France',
      mensSingles: true,
      womensSingles: true,
      mensDoubles: true,
      womensDoubles: true,
      mixedDoubles: true
    },
    {
      tournamentId: '3',
      title: 'Wimbledon',
      level: TournamentLevel.GrandSlam,
      surface: Surface.Grass,
      venue: 'All England Club',
      country: 'United Kingdom', 
      mensSingles: true,
      womensSingles: true,
      mensDoubles: true,
      womensDoubles: true,
      mixedDoubles: true
    }
  ]
}