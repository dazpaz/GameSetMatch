import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, finalize, of } from 'rxjs';
import { ITournament, Surface, TournamentLevel } from './tournament';
import { TournamentService } from './tournament-service';

type SortColumn = 'tournament' | 'location' | 'level' | 'surface';
type EventKey = 'mensSingles' | 'womensSingles' | 'mensDoubles' | 'womensDoubles' | 'mixedDoubles';
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
  readonly showInactive = signal(false);
  readonly searchTerm = signal('');
  readonly sort = signal<SortState>(DEFAULT_SORT);

  readonly filterEnabled = signal(false);
  readonly levelFilter = signal('');
  readonly surfaceFilter = signal('');

  readonly levelOptions = Object.values(TournamentLevel).filter(
    (value): value is TournamentLevel => typeof value === 'number',
  );

  readonly surfaceOptions = Object.values(Surface).filter(
    (value): value is Surface => typeof value === 'number',
  );

  readonly eventOptions: readonly { key: EventKey; label: string }[] = [
    { key: 'mensSingles', label: "Men's Singles" },
    { key: 'womensSingles', label: "Women's Singles" },
    { key: 'mensDoubles', label: "Men's Doubles" },
    { key: 'womensDoubles', label: "Women's Doubles" },
    { key: 'mixedDoubles', label: 'Mixed Doubles' },
  ];
  
  readonly selectedEvents = signal<ReadonlySet<EventKey>>(new Set());

  private readonly appliedSearchTerm = signal('');

  toggleEvent(key: EventKey, selected: boolean): void {
    this.selectedEvents.update((current) => {
      const next = new Set(current);
      if (selected) {
        next.add(key);
      } else {
        next.delete(key);
      }
      return next;
    });
  }

  toggleFilter(): void {
    const enabled = !this.filterEnabled();
    this.filterEnabled.set(enabled);
    this.selectedEvents.set(new Set());
    if (!enabled) {
      this.levelFilter.set('');
      this.surfaceFilter.set('');
    }
  }

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
    const level = this.filterEnabled() ? this.levelFilter() : '';
    const surface = this.filterEnabled() ? this.surfaceFilter() : '';
    const selectedEvents = this.filterEnabled() ? [...this.selectedEvents()] : [];
    const filteredTournaments = this.allTournaments().filter(
      (tournament) =>
        (this.showInactive() || tournament.isActive) &&
        tournament.title.toLowerCase().includes(this.appliedSearchTerm()) &&
        (level === '' || tournament.level === Number(level)) &&
        (surface === '' || tournament.surface === Number(surface)) &&
        selectedEvents.every((key) => tournament[key]),
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

  private sortValue(tournament: ITournament, column: SortColumn): string | number {
    switch (column) {
      case 'tournament':
        return tournament.title;
      case 'location':
        return tournament.location;
      case 'level':
        return tournament.level;
      case 'surface':
        return this.surfaceLabels[tournament.surface];
    }
  }

  readonly tournamentLevelLabels: Readonly<Record<TournamentLevel, string>> = {
    [TournamentLevel.GsmSlam]: 'Grand Slam',
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
