import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ITournament } from './tournament';

@Injectable({ providedIn: 'root' })
export class TournamentService {
  private readonly http = inject(HttpClient);
  private readonly tournamentsUrl = 'https://localhost:7288/tournaments';

  getTournaments(): Observable<readonly ITournament[]> {
    return this.http.get<readonly ITournament[]>(this.tournamentsUrl);
  }
}