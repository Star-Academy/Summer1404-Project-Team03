import { Button } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { SignOutDirective } from './sign-out.directive';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

describe('SignOutDirective', () => {
  it('should create an instance', () => {
    const mockButton = {} as Button;
    const mockAuthService = {} as AuthService;
    const mockMessage = {} as MessageService;
    const mockRouter = {} as Router;

    const directive = new SignOutDirective(
      mockButton,
      mockAuthService,
      mockMessage,
      mockRouter
    );

    expect(directive).toBeTruthy();
  });
});
