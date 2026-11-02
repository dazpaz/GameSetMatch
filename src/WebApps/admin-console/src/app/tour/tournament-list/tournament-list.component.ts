import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, finalize, of } from 'rxjs';
import { ITournament, Surface, TournamentLevel } from './tournament';
import { TournamentService } from './tournament-service';

type SortColumn = 'tournament' | 'level' | 'surface';
type SortDirection = 'ascending' | 'descending';

interface SortState {
  readonly column: SortColumn;
  readonly direction: SortDirection;
}

const DEFAULT_SORT: SortState = { column: 'tournament', direction: 'ascending' };

@Component({
  imports: [],
  selector: 'ac-tournament-list',
  templateUrl: './tournament-list.component.html',
})
export class TournamentListComponent {
  readonly errorMessage = signal<string | null>(null);
  readonly isLoading = signal(true);

  private readonly allTournaments = toSignal(
    inject(TournamentService)
      .getTournaments()
      .pipe(
        catchError(() => {
          this.errorMessage.set('Unable to retrieve tournaments');
          return of([] as readonly ITournament[]);
        }),
        finalize(() => this.isLoading.set(false)),
      ),
    { initialValue: [] as readonly ITournament[] },
  );

  readonly showInactive = signal(false);
  readonly searchTerm = signal('');
  readonly sort = signal<SortState>(DEFAULT_SORT);

  private readonly appliedSearchTerm = signal('');

  search(): void {
    this.appliedSearchTerm.set(this.searchTerm().trim().toLowerCase());
  }

  sortBy(column: SortColumn): void {
    this.sort.update((currentSort) => ({
      column,
      direction:
        currentSort.column === column && currentSort.direction === 'ascending'
          ? 'descending'
          : 'ascending',
    }));
  }

  readonly tournaments = computed(() => {
    const filteredTournaments = this.allTournaments().filter(
      (tournament) =>
        (this.showInactive() || tournament.isActive) &&
        tournament.title.toLowerCase().includes(this.appliedSearchTerm()),
    );

    const { column, direction } = this.sort();
    return [...filteredTournaments].sort((first, second) => {
      const firstValue = this.sortValue(first, column);
      const secondValue = this.sortValue(second, column);
      const comparison =
        typeof firstValue === 'number' && typeof secondValue === 'number'
          ? firstValue - secondValue
          : String(firstValue).localeCompare(String(secondValue));
      return direction === 'ascending' ? comparison : -comparison;
    });
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

  private sortValue(tournament: ITournament, column: SortColumn): string | number {
    switch (column) {
      case 'tournament':
        return tournament.title;
      case 'level':
        return tournament.level;
      case 'surface':
        return this.surfaceLabels[tournament.surface];
    }
  }
}
