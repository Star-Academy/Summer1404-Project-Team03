import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { LandingComponent } from './landing.component';

fdescribe('LandingComponent', () => {
  let component: LandingComponent;
  let fixture: ComponentFixture<LandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LandingComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the landing component', () => {
    expect(component).toBeTruthy();
  });

  it('should render the logo', () => {
    const img = fixture.debugElement.query(By.css('.header__logo__icon'));
    expect(img).toBeTruthy();
    expect(img.nativeElement.getAttribute('src')).toBe('/assets/logo.svg');
  });

  it('should display app name "DataWave"', () => {
    const span = fixture.debugElement.query(By.css('.header__logo__text'));
    expect(span.nativeElement.textContent).toContain('DataWave');
  });

  it('should have a sign-in button in the header', () => {
    const button = fixture.debugElement.query(By.css('p-button[end]'));
    expect(button).toBeTruthy();
    expect(button.nativeElement.getAttribute('label')).toBe('Sign in');
  });

  it('should have a "Get start analyst data" button', () => {
    const button = fixture.debugElement.query(By.css('.main__content__btn'));
    expect(button).toBeTruthy();
    expect(button.nativeElement.getAttribute('label')).toBe('Get start analyst data');
  });

  it('should render a lottie-player animation', () => {
    const lottie = fixture.debugElement.query(By.css('lottie-player'));
    expect(lottie).toBeTruthy();
    expect(lottie.nativeElement.getAttribute('src')).toBe('/assets/landing/AnalyticsCharacterAnimation.json');
  });
});