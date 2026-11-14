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
  readonly errorMessage = signal<string | null>(null);
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
    if (!this.details().title.trim()) {
      this.errorMessage.set('Tournament is required and cannot be blank.');
      return;
    }

    this.errorMessage.set(null);
    await this.router.navigateByUrl('/tour');
  }
}
