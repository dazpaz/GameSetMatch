import { httpResource } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ITournament } from './tournament';

@Injectable({
  providedIn: 'root',
})
export class TournamentService {
  private tournamentUrl = 'https://localhost:7141api/tournaments';
  
  tournamentResource = httpResource<ITournament[]>(() => this.tournamentUrl, { defaultValue: [] });
}
