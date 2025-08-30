import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';

import { NewUser, NewUserState, User } from '../../../../models/user.model';
import { MessageService } from 'primeng/api';
import { ManageUsersService } from '../../../../services/mange-users/manage-users.service';
import { catchError, exhaustMap, of, tap } from 'rxjs';

const initialUser: NewUserState = {
  error: null, isLoading: false, user: {
    username: '', email: '', firstName: '', lastName: '', password: ''
  }
}

@Injectable()
export class CreateUserStore extends ComponentStore<NewUserState> {

  constructor(
    private readonly messageService: MessageService,
    private readonly manageUserService: ManageUsersService
  ) {
    super(initialUser);
  }

  public readonly vm = this.selectSignal(s => s);

  public readonly createUser = this.effect<NewUser>((user$) => {
    return user$.pipe(
      tap(() => this.patchState({ isLoading: true })),
      exhaustMap((user: NewUser) => this.manageUserService.createUser(user).pipe(
        tap((user) => {
          this.patchState({ user: user, isLoading: false });
          this.showSuccessToast(user.username);
        }
        ),
        catchError(err => {
          this.patchState({ error: err.message, isLoading: false });

          if (err.status === 409) {
            this.showErrorToast("User already exists.");
          } else if (err.status === 500) {
            this.showErrorToast("Internal server error. Please try again later.");
          } else {
            this.showErrorToast(err.message || "Failed to create user.");
          }
          return of(err);
        })
      ))
    )
  })

  showSuccessToast(username: string): void {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: `User '${username}' created successfully!`,
    });
  }

  showErrorToast(message: string): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail: message,
      sticky: true,
    });
  }
}
