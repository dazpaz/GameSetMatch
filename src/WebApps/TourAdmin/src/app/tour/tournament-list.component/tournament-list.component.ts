import { Component, signal } from '@angular/core';
import { ITournament } from '../tournament';
import { TournamentData } from '../tournament-data';

@Component({
  selector: 'ta-tournament-list',
  imports: [],
  templateUrl: './tournament-list.component.html',
  styleUrl: './tournament-list.component.css',
})
export class TournamentListComponent {
  protected readonly tournaments = signal<ITournament[]>(TournamentData.tournaments);
}
