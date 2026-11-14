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
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the navigation bar', () => {
    const nav = fixture.nativeElement.querySelector('nav[aria-label="Main navigation"]');
    expect(nav).toBeTruthy();
  });

  it('should have navbar brand with correct href', () => {
    const navBrand = fixture.nativeElement.querySelector('.navbar-brand');
    expect(navBrand).toBeTruthy();
    expect(navBrand.textContent.trim()).toBe('Admin Console');
    expect(navBrand.getAttribute('href')).toBe('/tour');
  });

  it('should render all navigation links', () => {
    const navLinks = fixture.nativeElement.querySelectorAll('.nav-link');
    const expectedLinks = [
      { href: '/tour', text: 'Tour' },
      { href: '/venues', text: 'Venues' },
      { href: '/seasons', text: 'Seasons' },
      { href: '/tournaments', text: 'Tournaments' },
      { href: '/players', text: 'Players' },
      { href: '/user-admin', text: 'User Admin' },
      { href: '/logout', text: 'Logout' },
    ];

    expect(navLinks.length).toBe(expectedLinks.length);
    expectedLinks.forEach((link, index) => {
      expect(navLinks[index].getAttribute('href')).toBe(link.href);
      expect(navLinks[index].textContent.trim()).toBe(link.text);
    });
  });

  it('should display user name in navbar', () => {
    const userSpan = fixture.nativeElement.querySelector('.navbar-text');
    expect(userSpan).toBeTruthy();
    expect(userSpan.textContent).toBe('Dave Administrator');
  });

  it('should have responsive navbar toggler button', () => {
    const toggleButton = fixture.nativeElement.querySelector('.navbar-toggler');
    expect(toggleButton).toBeTruthy();
    expect(toggleButton.getAttribute('type')).toBe('button');
    expect(toggleButton.getAttribute('data-bs-toggle')).toBe('collapse');
    expect(toggleButton.getAttribute('data-bs-target')).toBe('#collapsibleNavbar');
    expect(toggleButton.getAttribute('aria-label')).toBe('Toggle navigation');
  });
});
