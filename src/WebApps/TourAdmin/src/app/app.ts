import { Component, signal } from '@angular/core';
import { TournamentList } from "./tour/tournament-list/tournament-list";

@Component({
  selector: 'app-root',
  imports: [TournamentList],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Tour Administration');
}
