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
    },
    {
      tournamentId: '4',
      title: 'Brisbane International',
      level: TournamentLevel.GSM500,
      surface: Surface.Hard,
      venue: 'Brisbane Tennis Centre',
      country: 'Australia',
      mensSingles: true,
      womensSingles: true,
      mensDoubles: true,
      womensDoubles: true,
      mixedDoubles: false
    },
    {
      tournamentId: '5',
      title: 'Nottingham Open',
      level: TournamentLevel.GSM250,
      surface: Surface.Hard,
      venue: 'Nottingham Tennis Centre',
      country: 'United Kingdom',
      mensSingles: false,
      womensSingles: true,
      mensDoubles: false,
      womensDoubles: true,
      mixedDoubles: false
    },
    {
      tournamentId: '6',
      title: 'Monte-Carlo Masters',
      level: TournamentLevel.GSM1000,
      surface: Surface.Clay,
      venue: 'Monte Carlo Country Club',
      country: 'Monaco',
      mensSingles: true,
      womensSingles: false,
      mensDoubles: true,
      womensDoubles: false,
      mixedDoubles: false
    }
  ]
}