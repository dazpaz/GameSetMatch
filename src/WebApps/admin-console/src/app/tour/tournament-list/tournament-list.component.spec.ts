import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TournamentListComponent } from './tournament-list.component';
import { ITournament, Surface, TournamentLevel } from './tournament';

const tournaments: ITournament[] = [
  {
    tournamentId: '1',
    title: 'Active Open',
    location: 'Paris, France',
    level: TournamentLevel.GsmSlam,
    surface: Surface.Clay,
    mensSingles: true,
    womensSingles: true,
    mensDoubles: true,
    womensDoubles: true,
    mixedDoubles: true,
    isActive: true,
  },
  {
    tournamentId: '2',
    title: 'Inactive Open',
    location: 'Lyon, France',
    level: TournamentLevel.Gsm125,
    surface: Surface.Carpet,
    mensSingles: true,
    womensSingles: false,
    mensDoubles: false,
    womensDoubles: false,
    mixedDoubles: false,
    isActive: false,
  },
];

describe('TournamentListComponent', () => {
  let component: TournamentListComponent;
  let fixture: ComponentFixture<TournamentListComponent>;
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TournamentListComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(TournamentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    http.expectOne('https://localhost:7288/tournaments').flush(tournaments);
    await fixture.whenStable();
  });

  afterEach(() => http.verify());

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render only active tournaments', () => {
    const renderedTournamentTitles = Array.from(
      fixture.nativeElement.querySelectorAll('tbody th[scope="row"] .tournament-title'),
      (cell: Element) => cell.textContent?.trim(),
    );

    expect(renderedTournamentTitles).toEqual(['Active Open']);
  });

  describe('sorting tournaments', () => {
    const titles = () =>
      Array.from(
        (fixture.nativeElement as HTMLElement).querySelectorAll(
          'tbody th[scope="row"] .tournament-title',
        ),
        (cell) => cell.textContent?.trim(),
      );
    const sortButton = (column: string) =>
      (fixture.nativeElement as HTMLElement).querySelector<HTMLButtonElement>(
        `button[aria-label="Sort by ${column}"]`,
      )!;
    const showAllTournaments = () => {
      (fixture.nativeElement as HTMLElement)
        .querySelector<HTMLInputElement>('#showInactive')!
        .click();
      fixture.detectChanges();
    };

    it('should start sorted by Tournament ascending', () => {
      showAllTournaments();

      expect(titles()).toEqual(['Active Open', 'Inactive Open']);
      expect(sortButton('Tournament').textContent?.trim()).toBe('Tournament ↑');
      expect(sortButton('Tournament').classList).toContain('fw-bold');
      expect(sortButton('Tournament').closest('th')?.getAttribute('aria-sort')).toBe('ascending');
      expect(sortButton('Level').closest('th')?.hasAttribute('aria-sort')).toBe(false);
      expect(
        (fixture.nativeElement as HTMLElement)
          .querySelector('tbody th[scope="row"]')
          ?.classList.contains('fw-normal'),
      ).toBe(true);
    });

    it('should sort by Level and reverse the order when clicked again', () => {
      showAllTournaments();
      sortButton('Level').click();
      fixture.detectChanges();

      expect(titles()).toEqual(['Active Open', 'Inactive Open']);
      expect(sortButton('Level').textContent?.trim()).toBe('Level ↑');
      expect(sortButton('Level').closest('th')?.getAttribute('aria-sort')).toBe('ascending');

      sortButton('Level').click();
      fixture.detectChanges();

      expect(titles()).toEqual(['Inactive Open', 'Active Open']);
      expect(sortButton('Level').textContent?.trim()).toBe('Level ↓');
      expect(sortButton('Level').closest('th')?.getAttribute('aria-sort')).toBe('descending');
    });

    it('should sort by Location and reverse the order when clicked again', () => {
      showAllTournaments();
      sortButton('Location').click();
      fixture.detectChanges();

      expect(titles()).toEqual(['Inactive Open', 'Active Open']);
      expect(sortButton('Location').textContent?.trim()).toBe('Location ↑');
      expect(sortButton('Location').closest('th')?.getAttribute('aria-sort')).toBe('ascending');

      sortButton('Location').click();
      fixture.detectChanges();

      expect(titles()).toEqual(['Active Open', 'Inactive Open']);
      expect(sortButton('Location').textContent?.trim()).toBe('Location ↓');
      expect(sortButton('Location').closest('th')?.getAttribute('aria-sort')).toBe('descending');
    });

    it('should sort by Surface and reverse the order when clicked again', () => {
      showAllTournaments();
      sortButton('Surface').click();
      fixture.detectChanges();

      expect(titles()).toEqual(['Inactive Open', 'Active Open']);
      expect(sortButton('Surface').textContent?.trim()).toBe('Surface ↑');

      sortButton('Surface').click();
      fixture.detectChanges();

      expect(titles()).toEqual(['Active Open', 'Inactive Open']);
      expect(sortButton('Surface').textContent?.trim()).toBe('Surface ↓');
    });
  });

  describe('show inactive tournaments switch', () => {
    const titles = () =>
      Array.from(
        (fixture.nativeElement as HTMLElement).querySelectorAll(
          'tbody th[scope="row"] .tournament-title',
        ),
        (cell) => cell.textContent?.trim(),
      );
    const toggle = () =>
      (fixture.nativeElement as HTMLElement).querySelector<HTMLInputElement>('#showInactive')!;

    it('should render a labelled Bootstrap switch above the table, initially off', () => {
      const element: HTMLElement = fixture.nativeElement;
      fixture.detectChanges();

      expect(toggle().type).toBe('checkbox');
      expect(toggle().getAttribute('role')).toBe('switch');
      expect(toggle().classList).toContain('form-check-input');
      expect(toggle().closest('.form-switch')).not.toBeNull();
      expect(toggle().checked).toBe(false);
      expect(element.querySelector('label[for="showInactive"]')?.textContent?.trim()).toBe(
        'Show inactive tournaments',
      );
      expect(
        toggle().compareDocumentPosition(element.querySelector('table')!) &
          Node.DOCUMENT_POSITION_FOLLOWING,
      ).toBeTruthy();
    });

    it('should show all tournaments when switched on and hide inactive when switched off', () => {
      toggle().click();
      fixture.detectChanges();
      expect(titles()).toEqual(['Active Open', 'Inactive Open']);

      toggle().click();
      fixture.detectChanges();
      expect(titles()).toEqual(['Active Open']);
    });

    it('should show an accessible disabled icon only for inactive tournaments', () => {
      toggle().click();
      fixture.detectChanges();

      const rows = (fixture.nativeElement as HTMLElement).querySelectorAll('tbody th[scope="row"]');
      const activeRow = rows[0];
      const inactiveRow = rows[1];
      const statusHeader = (fixture.nativeElement as HTMLElement).querySelectorAll('thead th')[1];
      const activeStatusCell = activeRow.nextElementSibling;
      const inactiveStatusCell = inactiveRow.nextElementSibling;

      expect(statusHeader.textContent?.trim()).toBe('');
      expect(statusHeader.getAttribute('aria-label')).toBe('Tournament status');
      expect(activeStatusCell?.querySelector('.fa-ban')).toBeNull();
      expect(inactiveStatusCell?.querySelector('.fa-solid.fa-ban')).not.toBeNull();
      expect(inactiveStatusCell?.querySelector('.fa-ban')?.getAttribute('aria-hidden')).toBe(
        'true',
      );
      expect(inactiveStatusCell?.querySelector('.visually-hidden')?.textContent?.trim()).toBe(
        'Inactive',
      );
    });
  });

  it('should not show an error alert when the request succeeds', () => {
    expect(fixture.nativeElement.querySelector('.alert.alert-danger')).toBeNull();
    expect(component.errorMessage()).toBeNull();
    expect(component.isLoading()).toBe(false);
  });
});

describe('TournamentListComponent while loading', () => {
  let fixture: ComponentFixture<TournamentListComponent>;
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TournamentListComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(TournamentListComponent);
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  it('should show a loading message until the request completes', () => {
    expect(fixture.componentInstance.isLoading()).toBe(true);
    expect(fixture.nativeElement.querySelector('tbody')?.textContent).toContain(
      'Loading tournaments...',
    );

    http.expectOne('https://localhost:7288/tournaments').flush([]);
    fixture.detectChanges();

    expect(fixture.componentInstance.isLoading()).toBe(false);
    expect(fixture.nativeElement.querySelector('tbody')?.textContent).toContain(
      'No tournaments are available.',
    );
  });
});

describe('TournamentListComponent when the API returns an error', () => {
  let component: TournamentListComponent;
  let fixture: ComponentFixture<TournamentListComponent>;
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TournamentListComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(TournamentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    http
      .expectOne('https://localhost:7288/tournaments')
      .flush('Server error', { status: 500, statusText: 'Internal Server Error' });
    await fixture.whenStable();
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  it('should set the error message', () => {
    expect(component.errorMessage()).toBe('Unable to retrieve tournaments');
    expect(component.isLoading()).toBe(false);
  });

  it('should show a red Bootstrap alert above the table', () => {
    const element: HTMLElement = fixture.nativeElement;
    const alert = element.querySelector('.alert.alert-danger');

    expect(alert?.textContent?.trim()).toBe('Unable to retrieve tournaments');
    expect(alert?.getAttribute('role')).toBe('alert');

    const table = element.querySelector('table')!;
    expect(alert!.compareDocumentPosition(table) & Node.DOCUMENT_POSITION_FOLLOWING).toBeTruthy();
  });

  it('should render no tournaments', () => {
    expect(fixture.nativeElement.querySelectorAll('tbody th[scope="row"]').length).toBe(0);
    expect(fixture.nativeElement.querySelector('tbody td')?.textContent).toContain(
      'No tournaments are available.',
    );
  });
});
