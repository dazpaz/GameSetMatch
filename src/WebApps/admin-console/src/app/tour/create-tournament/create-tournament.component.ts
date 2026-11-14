import { Component, inject, signal } from '@angular/core';
import { FormField, form } from '@angular/forms/signals';
import { Router, RouterLink } from '@angular/router';
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

type ValidationErrors = Partial<Record<'title' | 'location' | 'level' | 'surface' | 'events', string>>;

@Component({
  imports: [FormField, RouterLink],
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
  readonly validationErrors = signal<ValidationErrors>({});
  readonly levelOptions = Object.values(TournamentLevel).filter(
    (value): value is TournamentLevel => typeof value === 'number',
  );
  readonly surfaceOptions = Object.values(Surface).filter(
    (value): value is Surface => typeof value === 'number',
  );
  readonly tournamentLevelLabels = tournamentLevelLabels;
  readonly surfaceLabels = surfaceLabels;

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

    await this.router.navigateByUrl('/tour');
  }
}
