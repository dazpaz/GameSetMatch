import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppHeadingComponent } from './app-heading/app-heading.component';

@Component({
  imports: [RouterOutlet, AppHeadingComponent],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('admin-console');
}
