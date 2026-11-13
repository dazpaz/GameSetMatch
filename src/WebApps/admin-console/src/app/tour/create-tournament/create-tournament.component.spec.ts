import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';
import { of } from 'rxjs';
import { routes } from '../../app.routes';
import { TournamentService } from '../tournament-list/tournament-service';
import { CreateTournamentComponent } from './create-tournament.component';

describe('CreateTournamentComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideRouter(routes),
        { provide: TournamentService, useValue: { getTournaments: () => of([]) } },
      ],
    });
  });

  it.each(['Cancel', 'Save'])('returns to the tournament list when %s is clicked', async (label) => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);

    const button = Array.from(harness.routeNativeElement!.querySelectorAll('button')).find(
      (element) => element.textContent?.trim() === label,
    );
    expect(button).toBeDefined();
    button!.click();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour');
    expect(harness.routeNativeElement?.querySelector('table')).not.toBeNull();
  });
});
