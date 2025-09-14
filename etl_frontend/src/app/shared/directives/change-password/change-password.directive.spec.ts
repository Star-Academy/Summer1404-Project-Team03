import {ChangeDetectorRef, Renderer2 } from '@angular/core';
import { ChangePasswordDirective } from './change-password.directive';
import { Button } from 'primeng/button';
import { UsersService } from '../../services/user/users.service';
import { MessageService } from 'primeng/api';

describe('ChangePasswordDirective', () => {
  it('should create an instance', () => {
    const mockButton = {} as Button;
    const mockUsersService = {} as UsersService;
    const mockConfig = {} as MessageService;
    const mockRenderer = {} as ChangeDetectorRef;

    const directive = new ChangePasswordDirective(
      mockButton,
      mockUsersService,
      mockConfig,
      mockRenderer
    );

    expect(directive).toBeTruthy();
  });
});
