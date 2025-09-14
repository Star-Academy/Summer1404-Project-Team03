import {ChangeDetectorRef } from '@angular/core';
import { Button } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../services/auth/auth.service';
import { SignInDirective } from './sign-in.directive';
import { TestBed } from '@angular/core/testing';
import {of, throwError } from 'rxjs';

describe('SignInDirective', () => {
  let directive: SignInDirective;
  let mockButton: Button;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockMessageService: jasmine.SpyObj<MessageService>;
  let mockCdr: jasmine.SpyObj<ChangeDetectorRef>;

  beforeEach(() => {
    mockButton = {loading: false} as Button;
    mockAuthService = jasmine.createSpyObj('AuthService', ['getSignInUrl']);
    mockMessageService = jasmine.createSpyObj('MessageService', ['add']);
    mockCdr = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);

    TestBed.configureTestingModule({
      providers: [
        SignInDirective,
        {provide: Button, useValue: mockButton},
        {provide: AuthService, useValue: mockAuthService},
        {provide: MessageService, useValue: mockMessageService},
        {provide: ChangeDetectorRef, useValue: mockCdr},
      ]
    });

    TestBed.runInInjectionContext(() => {
      directive = new SignInDirective(
        mockButton,
        mockAuthService,
        mockMessageService,
        mockCdr,
      );
    })
  })

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });

  it('should call getSignInUrl and redirect on success', () => {
    const url = 'https://example.com';
    mockAuthService.getSignInUrl.and.returnValue(of({redirectUrl: url}))

    spyOn(directive as any, 'redirect');
    directive.handleClick();

    expect((directive as any).redirect).toHaveBeenCalledWith(url);
    expect(mockButton.loading).toBeFalse();
  });

  it('should show error message on failure', () => {
    mockAuthService.getSignInUrl.and.returnValue(throwError(() => new Error('fail')));

    directive.handleClick();

    expect(mockButton.loading).toBeFalse();
    expect(mockMessageService.add).toHaveBeenCalledWith({
      severity: 'error',
      summary: 'Signin Failed',
      detail: 'An error occurred during Singin. Please try again later.',
    });
  });

  it('should not call service if already loading', () => {
    directive['isLoading'].set(true);
    directive.handleClick();

    expect(mockAuthService.getSignInUrl).not.toHaveBeenCalled();
  });
});
