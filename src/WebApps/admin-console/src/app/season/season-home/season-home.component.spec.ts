import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SeasonHomeComponent } from './season-home.component';

describe('SeasonHomeComponent', () => {
  let component: SeasonHomeComponent;
  let fixture: ComponentFixture<SeasonHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SeasonHomeComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SeasonHomeComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
