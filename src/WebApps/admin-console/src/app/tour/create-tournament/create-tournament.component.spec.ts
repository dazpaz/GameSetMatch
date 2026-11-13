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

  it('updates tournament details from the form fields', async () => {
    const harness = await RouterTestingHarness.create();
    const component = await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    const element = harness.routeNativeElement!;
    const title = element.querySelector<HTMLInputElement>('#title')!;
    const location = element.querySelector<HTMLInputElement>('#location')!;
    const level = element.querySelector<HTMLSelectElement>('#level')!;
    const surface = element.querySelector<HTMLSelectElement>('#surface')!;

    title.value = 'Open';
    title.dispatchEvent(new Event('input'));
    location.value = 'London';
    location.dispatchEvent(new Event('input'));
    level.value = '1';
    level.dispatchEvent(new Event('input'));
    level.dispatchEvent(new Event('change'));
    surface.value = '2';
    surface.dispatchEvent(new Event('input'));
    surface.dispatchEvent(new Event('change'));
    element.querySelector<HTMLInputElement>('#mensSingles')!.click();
    harness.detectChanges();

    expect(component.details()).toMatchObject({
      title: 'Open',
      location: 'London',
      level: '1',
      surface: '2',
      mensSingles: true,
    });
    expect(component.details()).not.toHaveProperty('isActive');
    expect(element.querySelector('#isActive')).toBeNull();
    expect(element.textContent).toContain('Entered details will not be saved.');
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
