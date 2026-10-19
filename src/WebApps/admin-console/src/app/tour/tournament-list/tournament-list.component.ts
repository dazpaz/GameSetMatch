import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, of } from 'rxjs';
import { ITournament, Surface, TournamentLevel } from './tournament';
import { TournamentService } from './tournament-service';

@Component({
  imports: [],
  selector: 'ac-tournament-list',
  templateUrl: './tournament-list.component.html',
})
export class TournamentListComponent {
  readonly errorMessage = signal<string | null>(null);

  private readonly allTournaments = toSignal(
    inject(TournamentService)
      .getTournaments()
      .pipe(
        catchError(() => {
          this.errorMessage.set('Unable to retrieve tournaments');
          return of([] as readonly ITournament[]);
        }),
      ),
    { initialValue: [] as readonly ITournament[] },
  );

  readonly showInactive = signal(false);
  readonly searchTerm = signal('');

  private readonly appliedSearchTerm = signal('');

  search(): void {
    this.appliedSearchTerm.set(this.searchTerm().trim().toLowerCase());
  }

  readonly tournaments = computed(() => {
    return this.allTournaments().filter(
      (tournament) =>
        (this.showInactive() || tournament.isActive) &&
        tournament.title.toLowerCase().includes(this.appliedSearchTerm()),
    );
  });

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
