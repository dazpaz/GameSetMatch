import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TournamentListComponent } from './tournament-list.component';
import { TournamentService } from './tournament-service';

describe('TournamentListComponent', () => {
  let component: TournamentListComponent;
  let fixture: ComponentFixture<TournamentListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TournamentListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(TournamentListComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render only active tournaments', () => {
    const activeTournamentTitles = TestBed.inject(TournamentService)
      .getTournaments()
      .filter((tournament) => tournament.isActive)
      .map((tournament) => tournament.title);
    const renderedTournamentTitles = Array.from(
      fixture.nativeElement.querySelectorAll('tbody th[scope="row"]'),
      (cell: Element) => cell.textContent?.trim()
    );

    expect(renderedTournamentTitles).toEqual(activeTournamentTitles);
  });
});
