import { Component, inject, signal } from '@angular/core';
import { FormField, form } from '@angular/forms/signals';
import { Router } from '@angular/router';
import { ITournament, Surface, TournamentLevel, surfaceLabels, tournamentLevelLabels } from '../tournament-list/tournament';
import { TournamentService } from '../tournament-list/tournament-service';

type TournamentDetails = Omit<ITournament, 'tournamentId' | 'level' | 'surface' | 'isActive'> & {
  level: string;
  surface: string;
};

type ValidationErrors = Partial<Record<'title' | 'location' | 'level' | 'surface' | 'events', string>>;

@Component({
  imports: [FormField],
  selector: 'ac-create-tournament',
  templateUrl: './create-tournament.component.html',
})
export class CreateTournamentComponent {
  private readonly router = inject(Router);
  private readonly tournamentService = inject(TournamentService);
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
  readonly validationErrors = signal<ValidationErrors>({});
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
  readonly errorMessage = signal<string>('');
  readonly showErrorDialog = signal(false);

  async save(event: Event): Promise<void> {
    event.preventDefault();
    
    const errors: ValidationErrors = {};
    if (!this.details().title.trim()) {
      errors.title = 'Tournament is required and cannot be blank.';
    }
    if (!this.details().location.trim()) {
      errors.location = 'Location is required and cannot be blank.';
    }
    if (!this.details().level) {
      errors.level = 'Select a level.';
    }
    if (!this.details().surface) {
      errors.surface = 'Select a surface.';
    }
    const { mensSingles, womensSingles, mensDoubles, womensDoubles, mixedDoubles } = this.details();
    if (!mensSingles && !womensSingles && !mensDoubles && !womensDoubles && !mixedDoubles) {
      errors.events = 'Select at least one event.';
    }

    this.validationErrors.set(errors);
    if (Object.keys(errors).length > 0) {
      return;
    }

    const details = this.details();
    this.tournamentService.createTournament({
      title: details.title,
      location: details.location,
      level: Number(details.level),
      surface: Number(details.surface),
      mensSingles: details.mensSingles,
      womensSingles: details.womensSingles,
      mensDoubles: details.mensDoubles,
      womensDoubles: details.womensDoubles,
      mixedDoubles: details.mixedDoubles,
    }).subscribe({
      next: () => {
        this.router.navigateByUrl('/tour');
      },
      error: (err) => {
        console.error('Failed to create tournament:', err);
        this.errorMessage.set('An error occurred while saving the tournament.');
        this.showErrorDialog.set(true);
      }
    });
  }

  onErrorDialogOk(): void {
    this.showErrorDialog.set(false);
    this.errorMessage.set('');
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
