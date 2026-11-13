import { Component, inject, signal } from '@angular/core';
import { FormField, form } from '@angular/forms/signals';
import { Router } from '@angular/router';
import {
  ITournament,
  Surface,
  TournamentLevel,
  surfaceLabels,
  tournamentLevelLabels,
} from '../tournament-list/tournament';

type TournamentDetails = Omit<ITournament, 'tournamentId' | 'level' | 'surface' | 'isActive'> & {
  level: string;
  surface: string;
};

@Component({
  imports: [FormField],
  selector: 'ac-create-tournament',
  templateUrl: './create-tournament.component.html',
})
export class CreateTournamentComponent {
  private readonly router = inject(Router);
  readonly details = signal<TournamentDetails>({
    title: '',
    location: '',
    level: '',
    surface: '',
    mensSingles: false,
    womensSingles: false,
    mensDoubles: false,
    womensDoubles: false,
    mixedDoubles: false,
  });
  readonly tournamentForm = form(this.details);
  readonly levelOptions = Object.values(TournamentLevel).filter(
    (value): value is TournamentLevel => typeof value === 'number',
  );
  readonly surfaceOptions = Object.values(Surface).filter(
    (value): value is Surface => typeof value === 'number',
  );
  readonly tournamentLevelLabels = tournamentLevelLabels;
  readonly surfaceLabels = surfaceLabels;
  readonly isDirty = this.tournamentForm().dirty;
  readonly showUnsavedDialog = signal(false);

  async save(event: Event): Promise<void> {
    event.preventDefault();
    await this.router.navigateByUrl('/tour');
  }

  cancel(): void {
    if (this.isDirty()) {
      this.showUnsavedDialog.set(true);
    } else {
      this.navigateToTournamentList();
    }
  }

  onConfirmUnsavedChanges(confirm: boolean): void {
    this.showUnsavedDialog.set(false);
    if (confirm) {
      this.navigateToTournamentList();
    }
  }

  private async navigateToTournamentList(): Promise<void> {
    await this.router.navigateByUrl('/tour');
  }
}
