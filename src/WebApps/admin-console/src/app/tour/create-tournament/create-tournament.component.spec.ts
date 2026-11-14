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

  it.each(['', '   ', ' \t\n '])('blocks saving a blank tournament name %j', async (value) => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    const element = harness.routeNativeElement!;
    const title = element.querySelector<HTMLInputElement>('#title')!;
    title.value = value;
    title.dispatchEvent(new Event('input'));
    element.querySelector<HTMLFormElement>('form')!.dispatchEvent(
      new Event('submit', { bubbles: true, cancelable: true }),
    );
    harness.detectChanges();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
    const alert = element.querySelector('[role="alert"]');
    expect(alert?.textContent).toContain('Tournament is required');
    expect(element.querySelector('form')?.firstElementChild).toBe(alert);
    expect(title.getAttribute('aria-invalid')).toBe('true');
    expect(title.getAttribute('aria-describedby')).toBe('tournament-error');
  });

  it('allows saving after correcting a blank tournament name', async () => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    const element = harness.routeNativeElement!;
    const form = element.querySelector<HTMLFormElement>('form')!;
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    harness.detectChanges();
    expect(element.querySelector('[role="alert"]')).not.toBeNull();

    const title = element.querySelector<HTMLInputElement>('#title')!;
    title.value = '  Open  ';
    title.dispatchEvent(new Event('input'));
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour');
    expect(harness.routeNativeElement?.querySelector('table')).not.toBeNull();
  });

  it('returns to the tournament list when Cancel is clicked', async () => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);

    const button = Array.from(harness.routeNativeElement!.querySelectorAll('button')).find(
      (element) => element.textContent?.trim() === 'Cancel',
    );
    expect(button).toBeDefined();
    button!.click();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour');
    expect(harness.routeNativeElement?.querySelector('table')).not.toBeNull();
  });
});
