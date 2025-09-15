import { CheckRoutePipe } from './check-route.pipe';
import { Router } from '@angular/router';

describe('CheckRoutePipe', () => {
  let currentUrl: string;
  let mockRouter: Partial<Router>;
  let pipe: CheckRoutePipe;

  beforeEach(() => {
    currentUrl = '/dashboard';
    mockRouter = {
      get url() { return currentUrl; }
    };
    pipe = new CheckRoutePipe(mockRouter as Router);
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should return true if route matches', () => {
    expect(pipe.transform('/dashboard')).toBeTrue();
  });

  it('should return false if route does not match', () => {
    currentUrl = '/profile';
    expect(pipe.transform('/dashboard')).toBeFalse();
  });
});
