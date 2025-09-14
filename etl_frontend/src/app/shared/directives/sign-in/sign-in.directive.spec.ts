import {ChangeDetectorRef } from '@angular/core';
import { Button } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../services/auth/auth.service';
import { SignInDirective } from './sign-in.directive';

describe('SignInDirective', () => {
  it('should create an instance', () => {
    const mockButton = {} as Button;
    const mockAuthService = {} as AuthService;
    const mockMessage = {} as MessageService;
    const mockCdr = {} as ChangeDetectorRef;

    const directive = new SignInDirective(
      mockButton,
      mockAuthService,
      mockMessage,
      mockCdr
    );

    expect(directive).toBeTruthy();
  });
});
