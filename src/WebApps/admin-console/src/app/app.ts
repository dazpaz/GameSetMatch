import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppHeadingComponent } from './app-heading/app-heading.component';
import { TournamentListComponent } from './tour/tournament-list/tournament-list.component';

@Component({
  imports: [RouterOutlet, AppHeadingComponent, TournamentListComponent],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('admin-console');
}
