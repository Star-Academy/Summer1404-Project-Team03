import { CheckRoutePipe } from './check-route.pipe';
import { Router } from '@angular/router';

describe('CheckRoutePipe', () => {
  it('create an instance', () => {
    const mockRouter = { url: '/home' } as Router;
    const pipe = new CheckRoutePipe(mockRouter);
    expect(pipe).toBeTruthy();
  });

  it('should return true if route matches', () => {
    const mockRouter = { url: '/home' } as Router;
    const pipe = new CheckRoutePipe(mockRouter);

    expect(pipe.transform('/home')).toBeTrue();
  });

  it('should return false if route does not match', () => {
    const mockRouter = { url: '/about' } as Router;
    const pipe = new CheckRoutePipe(mockRouter);

    expect(pipe.transform('/home')).toBeFalse();
  });
});
