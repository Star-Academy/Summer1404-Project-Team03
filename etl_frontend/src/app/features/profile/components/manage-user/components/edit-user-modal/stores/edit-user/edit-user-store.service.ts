import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { EditUser, User, UserState } from '../../../../models/user.model';
import { catchError, exhaustMap, finalize, of, tap } from 'rxjs';
import { ManageUsersService } from '../../../../services/mange-users/manage-users.service';

const initialUser: UserState = {
  error: null,
  isLoading: false,
  user: {
    firstName: '',
    lastName: '',
    email: '',
    id: '',
    roles: [],
    username: ''
  }
};

@Injectable()
export class EditUserStore extends ComponentStore<UserState> {
  constructor(
    private readonly manageUserService: ManageUsersService
  ) {
    super(initialUser);
  }

  public readonly vm = this.selectSignal((s) => s);

  public readonly editUser = this.effect<{
    userNewInfo: EditUser;
    userId: string;
    onSuccess?: () => void;
  }>((user$) => {
    return user$.pipe(
      tap(() => this.patchState({ isLoading: true, error: null })),
      exhaustMap(({ userNewInfo, userId, onSuccess }) =>
        this.manageUserService.editUser(userNewInfo, userId).pipe(
          tap((res: User) => {
            this.patchState({ user: res });
            console.log(res);
            if (onSuccess) {
              onSuccess();
            }
          }),
          catchError((error) => {
            this.patchState({ error: error?.error?.message ?? 'Failed to update user' });
            return of(null);
          }),
          finalize(() => this.patchState({ isLoading: false }))
        )
      )
    );
  });
}
