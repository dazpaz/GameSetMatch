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
    const location = element.querySelector<HTMLInputElement>('#location')!;
    location.value = 'London';
    location.dispatchEvent(new Event('input'));
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
    expect(title.getAttribute('aria-describedby')).toBe('title-error');
    expect(location.hasAttribute('aria-invalid')).toBe(false);
    expect(alert?.textContent).not.toContain('Location is required');
  });

  it.each(['', '   ', ' \t '])('blocks saving a blank location %j', async (value) => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    const element = harness.routeNativeElement!;
    const title = element.querySelector<HTMLInputElement>('#title')!;
    const location = element.querySelector<HTMLInputElement>('#location')!;
    title.value = 'Open';
    title.dispatchEvent(new Event('input'));
    location.value = value;
    location.dispatchEvent(new Event('input'));
    element.querySelector<HTMLFormElement>('form')!.dispatchEvent(
      new Event('submit', { bubbles: true, cancelable: true }),
    );
    harness.detectChanges();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
    const alert = element.querySelector('[role="alert"]');
    expect(alert?.textContent).toContain('Location is required');
    expect(element.querySelector('form')?.firstElementChild).toBe(alert);
    expect(location.getAttribute('aria-invalid')).toBe('true');
    expect(location.getAttribute('aria-describedby')).toBe('location-error');
    expect(title.hasAttribute('aria-invalid')).toBe(false);
    expect(title.hasAttribute('aria-describedby')).toBe(false);
  });

  it.each([
    { missing: 'level', selected: 'surface', message: 'Select a level.' },
    { missing: 'surface', selected: 'level', message: 'Select a surface.' },
  ])('blocks saving when $missing is not selected', async ({ missing, selected, message }) => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    const element = harness.routeNativeElement!;
    const title = element.querySelector<HTMLInputElement>('#title')!;
    const location = element.querySelector<HTMLInputElement>('#location')!;
    title.value = 'Open';
    title.dispatchEvent(new Event('input'));
    location.value = 'London';
    location.dispatchEvent(new Event('input'));
    const selectedField = element.querySelector<HTMLSelectElement>(`#${selected}`)!;
    selectedField.value = '0';
    selectedField.dispatchEvent(new Event('input'));
    selectedField.dispatchEvent(new Event('change'));
    element.querySelector<HTMLFormElement>('form')!.dispatchEvent(
      new Event('submit', { bubbles: true, cancelable: true }),
    );
    harness.detectChanges();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
    const alert = element.querySelector('[role="alert"]');
    expect(element.querySelector('form')?.firstElementChild).toBe(alert);
    expect(alert?.textContent).toContain(message);
    const missingField = element.querySelector<HTMLSelectElement>(`#${missing}`)!;
    expect(missingField.getAttribute('aria-invalid')).toBe('true');
    expect(missingField.getAttribute('aria-describedby')).toBe(`${missing}-error`);
    expect(selectedField.hasAttribute('aria-invalid')).toBe(false);
    expect(selectedField.hasAttribute('aria-describedby')).toBe(false);
  });

  it('reports all errors and allows saving after correcting them', async () => {
    const harness = await RouterTestingHarness.create();
    await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    const element = harness.routeNativeElement!;
    const form = element.querySelector<HTMLFormElement>('form')!;
    const title = element.querySelector<HTMLInputElement>('#title')!;
    const location = element.querySelector<HTMLInputElement>('#location')!;
    const level = element.querySelector<HTMLSelectElement>('#level')!;
    const surface = element.querySelector<HTMLSelectElement>('#surface')!;
    const events = element.querySelector<HTMLFieldSetElement>('fieldset')!;
    expect(form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }))).toBe(false);
    harness.detectChanges();
    await harness.fixture.whenStable();
    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
    const alert = element.querySelector('[role="alert"]');
    expect(alert?.textContent).toContain('Tournament is required');
    expect(alert?.textContent).toContain('Location is required');
    expect(alert?.textContent).toContain('Select a level.');
    expect(alert?.textContent).toContain('Select a surface.');
    expect(alert?.textContent).toContain('Select at least one event.');
    expect(title.getAttribute('aria-invalid')).toBe('true');
    expect(title.getAttribute('aria-describedby')).toBe('title-error');
    expect(location.getAttribute('aria-invalid')).toBe('true');
    expect(location.getAttribute('aria-describedby')).toBe('location-error');
    expect(level.getAttribute('aria-invalid')).toBe('true');
    expect(level.getAttribute('aria-describedby')).toBe('level-error');
    expect(surface.getAttribute('aria-invalid')).toBe('true');
    expect(surface.getAttribute('aria-describedby')).toBe('surface-error');
    expect(events.getAttribute('aria-describedby')).toBe('events-error');

    title.value = '  Open  ';
    title.dispatchEvent(new Event('input'));
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    harness.detectChanges();
    expect(element.querySelector('[role="alert"]')?.textContent).toContain('Location is required');
    expect(element.querySelector('[role="alert"]')?.textContent).not.toContain('Tournament is required');
    expect(title.hasAttribute('aria-invalid')).toBe(false);
    expect(title.hasAttribute('aria-describedby')).toBe(false);
    expect(location.getAttribute('aria-invalid')).toBe('true');

    location.value = '  London  ';
    location.dispatchEvent(new Event('input'));
    level.value = '0';
    level.dispatchEvent(new Event('input'));
    level.dispatchEvent(new Event('change'));
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    harness.detectChanges();
    expect(element.querySelector('[role="alert"]')?.textContent).toContain('Select a surface.');
    expect(element.querySelector('[role="alert"]')?.textContent).not.toContain('Select a level.');
    expect(level.hasAttribute('aria-invalid')).toBe(false);
    expect(level.hasAttribute('aria-describedby')).toBe(false);
    expect(surface.getAttribute('aria-invalid')).toBe('true');
    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');

    surface.value = '0';
    surface.dispatchEvent(new Event('input'));
    surface.dispatchEvent(new Event('change'));
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    harness.detectChanges();
    await harness.fixture.whenStable();
    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
    expect(element.querySelector('[role="alert"]')?.textContent).toContain('Select at least one event.');
    expect(element.querySelector('[role="alert"]')?.querySelectorAll('li')).toHaveLength(1);

    element.querySelector<HTMLInputElement>('#mensSingles')!.click();
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour');
    expect(harness.routeNativeElement?.querySelector('table')).not.toBeNull();
  });

  it.each(['mensSingles', 'womensSingles', 'mensDoubles', 'womensDoubles', 'mixedDoubles'])(
    'accepts %s as the only selected event and rejects deselection',
    async (eventId) => {
      const harness = await RouterTestingHarness.create();
      const component = await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
      component.details.update((details) => ({
        ...details,
        title: 'Open',
        location: 'London',
        level: '0',
        surface: '0',
      }));
      harness.detectChanges();
      const element = harness.routeNativeElement!;
      const event = element.querySelector<HTMLInputElement>(`#${eventId}`)!;
      const form = element.querySelector<HTMLFormElement>('form')!;
      event.click();
      event.click();
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      harness.detectChanges();
      await harness.fixture.whenStable();
      expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
      expect(element.querySelector('[role="alert"]')?.textContent).toContain('Select at least one event.');

      event.click();
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      await harness.fixture.whenStable();
      expect(TestBed.inject(Router).url).toBe('/tour');
    },
  );

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
