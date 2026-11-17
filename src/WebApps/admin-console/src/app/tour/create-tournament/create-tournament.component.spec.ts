import { TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { RouterTestingHarness } from '@angular/router/testing';
import { of, Subject, throwError } from 'rxjs';
import { routes } from '../../app.routes';
import { TournamentService } from '../tournament-list/tournament-service';
import { CreateTournamentComponent } from './create-tournament.component';

describe('CreateTournamentComponent', () => {
  let createTournament: ReturnType<typeof vi.fn>;
  let getTournament: ReturnType<typeof vi.fn>;
  let updateTournament: ReturnType<typeof vi.fn>;

  beforeEach(() => {
    createTournament = vi.fn(() => of(''));
    getTournament = vi.fn();
    updateTournament = vi.fn(() => of(undefined));
    TestBed.configureTestingModule({
      providers: [
        provideRouter(routes),
        {
          provide: TournamentService,
          useValue: { getTournaments: () => of([]), getTournament, createTournament, updateTournament },
        },
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
    expect(element.textContent).toContain('Create Tournament');
    expect(element.textContent).not.toContain('will not be saved');
  });

  it('loads and updates the selected tournament', async () => {
    getTournament.mockReturnValue(
      of({
        tournamentId: 'tournament-1',
        title: 'Open',
        location: 'London',
        level: 1,
        surface: 2,
        mensSingles: true,
        womensSingles: false,
        mensDoubles: false,
        womensDoubles: false,
        mixedDoubles: false,
        isActive: true,
      }),
    );
    const harness = await RouterTestingHarness.create();
    const component = await harness.navigateByUrl(
      '/tour/edit-tournament/tournament-1',
      CreateTournamentComponent,
    );
    harness.detectChanges();

    expect(getTournament).toHaveBeenCalledWith('tournament-1');
    expect(component.details()).toMatchObject({
      title: 'Open',
      location: 'London',
      level: '1',
      surface: '2',
      mensSingles: true,
    });
    expect(harness.routeNativeElement?.textContent).toContain('Edit Tournament');

    const title = harness.routeNativeElement!.querySelector<HTMLInputElement>('#title')!;
    title.value = 'Updated Open';
    title.dispatchEvent(new Event('input'));
    harness.routeNativeElement!.querySelector<HTMLFormElement>('form')!.dispatchEvent(
      new Event('submit', { bubbles: true, cancelable: true }),
    );
    await harness.fixture.whenStable();

    expect(updateTournament).toHaveBeenCalledWith('tournament-1', {
      title: 'Updated Open',
      location: 'London',
      level: 1,
      surface: 2,
      mensSingles: true,
      womensSingles: false,
      mensDoubles: false,
      womensDoubles: false,
      mixedDoubles: false,
    });
    expect(TestBed.inject(Router).url).toBe('/tour');
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
    const response = new Subject<string>();
    createTournament.mockReturnValue(response);
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
    expect(createTournament).not.toHaveBeenCalled();

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
    expect(createTournament).not.toHaveBeenCalled();

    element.querySelector<HTMLInputElement>('#mensSingles')!.click();
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    expect(createTournament).toHaveBeenCalledOnce();
    expect(createTournament).toHaveBeenCalledWith({
      title: '  Open  ',
      location: '  London  ',
      level: 0,
      surface: 0,
      mensSingles: true,
      womensSingles: false,
      mensDoubles: false,
      womensDoubles: false,
      mixedDoubles: false,
    });
    expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');

    response.next('');
    response.complete();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour');
    expect(harness.routeNativeElement?.querySelector('table')).not.toBeNull();
  });

  it('shows an error dialog after a failed save and keeps the form available for retry', async () => {
    const error = new Error('Save failed');
    createTournament.mockReturnValueOnce(throwError(() => error));
    const consoleError = vi.spyOn(console, 'error').mockImplementation(() => {});
    try {
      const harness = await RouterTestingHarness.create();
      const component = await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
      component.details.update((details) => ({
        ...details,
        title: 'Open',
        location: 'London',
        level: '1',
        surface: '2',
        womensDoubles: true,
      }));
      harness.detectChanges();
      const element = harness.routeNativeElement!;
      const form = element.querySelector<HTMLFormElement>('form')!;

      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      harness.detectChanges();

      expect(createTournament).toHaveBeenCalledWith({
        title: 'Open',
        location: 'London',
        level: 1,
        surface: 2,
        mensSingles: false,
        womensSingles: false,
        mensDoubles: false,
        womensDoubles: true,
        mixedDoubles: false,
      });
      const dialog = element.querySelector<HTMLElement>('[role="dialog"]')!;
      expect(dialog.textContent).toContain('An error occurred while saving the tournament.');
      expect(consoleError).toHaveBeenCalledWith('Failed to create tournament:', error);
      expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
      expect(component.details().title).toBe('Open');
      expect(component.details().womensDoubles).toBe(true);

      const ok = Array.from(dialog.querySelectorAll('button')).find(
        (button) => button.textContent?.trim() === 'OK',
      );
      expect(ok).toBeDefined();
      ok!.click();
      harness.detectChanges();
      expect(element.querySelector('[role="dialog"]')).toBeNull();
      expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');
      expect(component.details().title).toBe('Open');

      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      await harness.fixture.whenStable();
      expect(createTournament).toHaveBeenCalledTimes(2);
      expect(TestBed.inject(Router).url).toBe('/tour');
    } finally {
      consoleError.mockRestore();
    }
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
    const component = await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
    expect(component.isDirty()).toBe(false);

    const button = Array.from(harness.routeNativeElement!.querySelectorAll('button')).find(
      (element) => element.textContent?.trim() === 'Cancel',
    );
    expect(button).toBeDefined();
    button!.click();
    await harness.fixture.whenStable();

    expect(TestBed.inject(Router).url).toBe('/tour');
    expect(harness.routeNativeElement?.querySelector('table')).not.toBeNull();
  });

  it.each(['title', 'level', 'mensSingles'])(
    'marks the form dirty when %s changes and confirms before leaving',
    async (field) => {
      const harness = await RouterTestingHarness.create();
      const component = await harness.navigateByUrl('/tour/create-tournament', CreateTournamentComponent);
      const element = harness.routeNativeElement!;
      const input = element.querySelector<HTMLInputElement | HTMLSelectElement>(`#${field}`)!;

      expect(component.isDirty()).toBe(false);
      if (field === 'mensSingles') {
        input.click();
      } else {
        input.value = field === 'title' ? 'Open' : '1';
        input.dispatchEvent(new Event('input'));
        input.dispatchEvent(new Event('change'));
      }
      harness.detectChanges();

      expect(component.isDirty()).toBe(true);
      const button = (label: string) => Array.from(element.querySelectorAll('button')).find(
        (candidate) => candidate.textContent?.trim() === label,
      )!;
      button('Cancel').click();
      harness.detectChanges();
      expect(element.querySelector('[role="dialog"]')).not.toBeNull();

      button('No').click();
      harness.detectChanges();
      expect(element.querySelector('[role="dialog"]')).toBeNull();
      expect(component.isDirty()).toBe(true);
      expect(TestBed.inject(Router).url).toBe('/tour/create-tournament');

      button('Cancel').click();
      harness.detectChanges();
      button('Yes').click();
      await harness.fixture.whenStable();
      expect(TestBed.inject(Router).url).toBe('/tour');
    },
  );
});
