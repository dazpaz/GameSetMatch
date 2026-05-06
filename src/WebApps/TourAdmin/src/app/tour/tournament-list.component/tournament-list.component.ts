import { Component, inject, signal } from '@angular/core';
import { ITournament } from '../tournament';
import { TournamentService } from '../tournament.service';

@Component({
  selector: 'ta-tournament-list',
  imports: [],
  templateUrl: './tournament-list.component.html',
  styleUrl: './tournament-list.component.css',
})
export class TournamentListComponent {
  private tournamentService = inject(TournamentService);

  tournaments = this.tournamentService.tournamentResource.value;
}
