import { Button } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { SignOutDirective } from './sign-out.directive';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import {fakeAsync, TestBed, tick } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

describe('SignOutDirective', () => {
  let directive: SignOutDirective;
  let mockButton: Button;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockMessageService: jasmine.SpyObj<MessageService>;
  let mockRouter: Router;

  beforeEach(() => {
    mockButton = {loading: false} as Button;
    mockAuthService = jasmine.createSpyObj('AuthService', ['signOut']);
    mockMessageService = jasmine.createSpyObj('MessageService', ['add']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    TestBed.configureTestingModule({
      providers: [
        SignOutDirective,
        {provide: Button, useValue: mockButton},
        {provide: Router, useValue: mockRouter},
        {provide: AuthService, useValue: mockAuthService},
        {provide: MessageService, useValue: mockMessageService},
      ]
    });

    TestBed.runInInjectionContext(() => {
      directive = new SignOutDirective(
        mockButton,
        mockAuthService,
        mockMessageService,
        mockRouter,
      );
    })
  })

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });

  it('should call signOut and navigate on success', fakeAsync(() => {
    mockAuthService.signOut.and.returnValue(of({}));

    directive.handleClick();

    expect(mockMessageService.add).toHaveBeenCalledWith({
      severity: 'info',
      summary: 'You are signedout.',
    });

    tick(1000);

    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('landing');
    expect(mockButton.loading).toBeFalse();
  }));

  it('should reset loading and not navigate on error', () => {
    mockAuthService.signOut.and.returnValue(throwError(() => new Error('fail')));

    directive.handleClick();

    expect(mockButton.loading).toBeFalse();
    expect(mockMessageService.add).not.toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).not.toHaveBeenCalled();
  });

  it('should not call service if already loading', () => {
    (directive as any).isLoading.set(true);

    directive.handleClick();

    expect(mockAuthService.signOut).not.toHaveBeenCalled();
  });
});
