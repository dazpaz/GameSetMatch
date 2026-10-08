import { Component } from '@angular/core';
import { ITournament, Surface, TournamentLevel } from './tournament';

@Component({
  imports: [],
  selector: 'ac-tournament-list',
  templateUrl: './tournament-list.component.html',
})
export class TournamentListComponent {
  readonly tournaments: readonly ITournament[] = [
    {
      tournamentId: 1,
      title: 'Australian Open',
      level: TournamentLevel.GrandSlam,
      surface: Surface.Hard,
      mensSingles: true,
      womensSingles: true,
      mensDoubles: true,
      womensDoubles: true,
      mixedDoubles: true,
    },
    {
      tournamentId: 2,
      title: 'Monte-Carlo Masters',
      level: TournamentLevel.Masters1000,
      surface: Surface.Clay,
      mensSingles: true,
      womensSingles: false,
      mensDoubles: true,
      womensDoubles: false,
      mixedDoubles: false,
    },
    {
      tournamentId: 3,
      title: 'Wimbledon',
      level: TournamentLevel.GrandSlam,
      surface: Surface.Grass,
      mensSingles: true,
      womensSingles: true,
      mensDoubles: true,
      womensDoubles: true,
      mixedDoubles: true,
    },
  ];

  readonly tournamentLevelLabels: Readonly<Record<TournamentLevel, string>> = {
    [TournamentLevel.GrandSlam]: 'Grand Slam',
    [TournamentLevel.Masters1000]: 'Masters 1000',
    [TournamentLevel.ATP500]: 'ATP 500',
    [TournamentLevel.ATP250]: 'ATP 250',
  };

  readonly surfaceLabels: Readonly<Record<Surface, string>> = {
    [Surface.Grass]: 'Grass',
    [Surface.Clay]: 'Clay',
    [Surface.Hard]: 'Hard',
    [Surface.Carpet]: 'Carpet',
  };
}
