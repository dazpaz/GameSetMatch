import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ITournament } from './tournament';

interface TournamentRequest {
  title: string;
  location: string;
  level: number;
  surface: number;
  mensSingles: boolean;
  womensSingles: boolean;
  mensDoubles: boolean;
  womensDoubles: boolean;
  mixedDoubles: boolean;
}

@Injectable({ providedIn: 'root' })
export class TournamentService {
  private readonly http = inject(HttpClient);
  private readonly tournamentsUrl = 'https://localhost:7288/tournaments';

  getTournaments(): Observable<readonly ITournament[]> {
    return this.http.get<readonly ITournament[]>(this.tournamentsUrl);
  }

  getTournament(id: string): Observable<ITournament> {
    return this.http.get<ITournament>(`${this.tournamentsUrl}/${id}`);
  }

  createTournament(tournament: TournamentRequest): Observable<string> {
    return this.http.post<string>(this.tournamentsUrl, tournament);
  }

  updateTournament(id: string, tournament: TournamentRequest): Observable<void> {
    return this.http.put<void>(`${this.tournamentsUrl}/${id}`, tournament);
  }
}