import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppHeadingComponent } from './app-heading.component';

describe('AppHeadingComponent', () => {
  let component: AppHeadingComponent;
  let fixture: ComponentFixture<AppHeadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppHeadingComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(AppHeadingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the admin console heading', () => {
    expect((fixture.nativeElement as HTMLElement).querySelector('h1')?.textContent).toBe(
      'Welcome to the Admin Console',
    );
  });
});
