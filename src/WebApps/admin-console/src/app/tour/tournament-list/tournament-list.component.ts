import { Component, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ITournament, Surface, TournamentLevel } from './tournament';
import { TournamentService } from './tournament-service';

@Component({
  imports: [],
  selector: 'ac-tournament-list',
  templateUrl: './tournament-list.component.html',
})
export class TournamentListComponent {
  private readonly allTournaments = toSignal(inject(TournamentService).getTournaments(), {
    initialValue: [] as readonly ITournament[],
  });

  readonly tournaments = computed(() =>
    this.allTournaments().filter((tournament) => tournament.isActive),
  );

  readonly tournamentLevelLabels: Readonly<Record<TournamentLevel, string>> = {
    [TournamentLevel.GsmSlam]: 'GSM Slam',
    [TournamentLevel.Gsm1000]: 'Masters 1000',
    [TournamentLevel.Gsm500]: 'GSM 500',
    [TournamentLevel.Gsm250]: 'GSM 250',
    [TournamentLevel.Gsm125]: 'GSM 125',
  };

  readonly surfaceLabels: Readonly<Record<Surface, string>> = {
    [Surface.Grass]: 'Grass',
    [Surface.Clay]: 'Clay',
    [Surface.Hard]: 'Hard',
    [Surface.Carpet]: 'Carpet',
  };
}
