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
});
