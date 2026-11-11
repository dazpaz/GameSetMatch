import { Routes } from '@angular/router';
import { TournamentListComponent } from './tour/tournament-list/tournament-list.component';
import { VenueListComponent } from './venue/venue-list/venue-list.component';
import { SeasonHomeComponent } from './season/season-home/season-home.component';

export const routes: Routes = [
  {
    path: '',
    component: TournamentListComponent
  },
  {
    path: 'venues',
    component: VenueListComponent
  },
  {
    path: 'seasons',
    component: SeasonHomeComponent
  }
  // Tournaments
  // Players
  // User Administration
];
