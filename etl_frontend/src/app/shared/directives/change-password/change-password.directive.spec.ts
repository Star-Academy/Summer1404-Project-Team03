import {ChangeDetectorRef} from '@angular/core';
import {ChangePasswordDirective} from './change-password.directive';
import {Button} from 'primeng/button';
import {UsersService} from '../../services/user/users.service';
import {MessageService} from 'primeng/api';
import {of, throwError} from 'rxjs';
import {TestBed} from '@angular/core/testing';

describe('ChangePasswordDirective', () => {
  let directive: ChangePasswordDirective;
  let mockButton: Button;
  let mockUsersService: jasmine.SpyObj<UsersService>;
  let mockMessageService: jasmine.SpyObj<MessageService>;
  let mockCdr: jasmine.SpyObj<ChangeDetectorRef>

  beforeEach(() => {
    mockButton = {loading: false} as Button;
    mockUsersService = jasmine.createSpyObj('UsersService', ['getChangePasswordUrl']);
    mockMessageService = jasmine.createSpyObj('MessageService', ['add']);
    mockCdr = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);

    TestBed.configureTestingModule({
      providers: [
        ChangePasswordDirective,
        {provide: Button, useValue: mockButton},
        {provide: UsersService, useValue: mockUsersService},
        {provide: MessageService, useValue: mockMessageService},
        {provide: ChangeDetectorRef, useValue: mockCdr},
      ],
    });

    TestBed.runInInjectionContext(() => {
      directive = new ChangePasswordDirective(
        mockButton,
        mockUsersService,
        mockMessageService,
        mockCdr
      );
    });
  })

  it('should create an instance', () => {
    expect(directive).toBeTruthy();
  });

  it('should call changePassword and redirect on success', () => {
    const url = 'https://example.com';
    mockUsersService.getChangePasswordUrl.and.returnValue(of({changePasswordUrl: url}));

    spyOn(directive as any, 'redirect');
    directive.handleClick();

    expect((directive as any).redirect).toHaveBeenCalledWith(url);
    expect(mockButton.loading).toBeFalse();
  });

  it('should show error message on failure', () => {
    mockUsersService.getChangePasswordUrl.and.returnValue(throwError(() => new Error('fail')));

    directive.handleClick();

    expect(mockButton.loading).toBeFalse();
    expect(mockMessageService.add).toHaveBeenCalledWith({
      severity: 'error',
      summary: 'Request failed.',
      detail: 'An error occurred during change password. Please try again later.',
    });
  });

  it('should not call service if already loading', () => {
    directive['isLoading'].set(true);
    directive.handleClick();

    expect(mockUsersService.getChangePasswordUrl).not.toHaveBeenCalled();
  });
});
