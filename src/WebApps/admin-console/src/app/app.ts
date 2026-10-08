import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TournamentListComponent } from './tour/tournament-list/tournament-list.component';

@Component({
  imports: [RouterOutlet, TournamentListComponent],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('admin-console');
}
