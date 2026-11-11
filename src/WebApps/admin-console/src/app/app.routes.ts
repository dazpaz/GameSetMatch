import { Routes } from '@angular/router';
import { TournamentListComponent } from './tour/tournament-list/tournament-list.component';
import { VenueListComponent } from './venue/venue-list/venue-list.component';
import { SeasonHomeComponent } from './season/season-home/season-home.component';
import { NotFoundComponent } from './not-found/not-found.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'tour',
    pathMatch: 'full'
  },
  {
    path: 'tour',
    component: TournamentListComponent
  },
  {
    path: 'venues',
    component: VenueListComponent
  },
  {
    path: 'seasons',
    component: SeasonHomeComponent
  },
  // Tournaments
  // Players
  // User Administration
  {
    path: '**',
    component: NotFoundComponent
  }

];
