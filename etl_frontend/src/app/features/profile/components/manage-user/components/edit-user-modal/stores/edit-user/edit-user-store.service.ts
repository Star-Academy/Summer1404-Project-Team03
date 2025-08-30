import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { EditUser, User, UserState } from '../../../../models/user.model';
import { catchError, exhaustMap, of, tap } from 'rxjs';
import { ManageUsersService } from '../../../../services/mange-users/manage-users.service';

const initialUser: UserState = {
  email: '',
  error: null,
  firstName: '',
  id: '',
  isLoading: false,
  lastName: '',
  roles: [],
  username: ''
}

@Injectable()
export class EditUserStore extends ComponentStore<UserState> {

  constructor(
    private readonly manageUserService: ManageUsersService
  ) {
    super(initialUser);
  }

  public readonly vm = this.selectSignal(s => s);

  public readonly editUser = this.effect<{userNewInfo: EditUser, userId: string, onSuccess?: () => void}>((user$) => {
    return user$.pipe(
      tap(() => this.patchState({ isLoading: true })),
      exhaustMap(({userNewInfo, userId}) => this.manageUserService.editUser(userNewInfo, userId).pipe(
        tap((res) => console.log('user updated successfuly res : ', res)),
        catchError(error => {
          this.patchState({isLoading: false})
          console.log("fail to update user, error: ", error);
          return of(error);
        })
      ))
    )
  })
}
