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
      fixture.nativeElement.querySelectorAll('tbody th[scope="row"]'),
      (cell: Element) => cell.textContent?.trim()
    );

    expect(renderedTournamentTitles).toEqual(['Active Open']);
  });

  it('should not show an error alert when the request succeeds', () => {
    expect(fixture.nativeElement.querySelector('.alert.alert-danger')).toBeNull();
    expect(component.errorMessage()).toBeNull();
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
  });

  it('should show a red Bootstrap alert above the table', () => {
    const element: HTMLElement = fixture.nativeElement;
    const alert = element.querySelector('.alert.alert-danger');

    expect(alert?.textContent?.trim()).toBe('Unable to retrieve tournaments');
    expect(alert?.getAttribute('role')).toBe('alert');

    const table = element.querySelector('table')!;
    expect(
      alert!.compareDocumentPosition(table) & Node.DOCUMENT_POSITION_FOLLOWING,
    ).toBeTruthy();
  });

  it('should render no tournaments', () => {
    expect(fixture.nativeElement.querySelectorAll('tbody th[scope="row"]').length).toBe(0);
    expect(fixture.nativeElement.querySelector('tbody td')?.textContent).toContain(
      'No tournaments are available.',
    );
  });
});
