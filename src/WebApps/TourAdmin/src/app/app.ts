import { Component, signal } from '@angular/core';
import { TournamentListComponent } from "./tour/tournament-list.component/tournament-list.component";

@Component({
  selector: 'app-root',
  imports: [TournamentListComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Tour Administration');
}
