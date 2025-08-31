import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';

import { baseState } from '../../../../models/user.model';
import { catchError, exhaustMap, of, tap, throwError } from 'rxjs';
import { ManageUsersService } from '../../../../services/mange-users/manage-users.service';
import { MessageService } from 'primeng/api';
import { UserListStore } from '../../../../stores/user-list/user-list-store.service';


const initialState: baseState = { isLoading: false, error: null }

@Injectable()
export class DeleteUserStore extends ComponentStore<baseState> {

  constructor(
    private readonly manageUsersService: ManageUsersService,
    private readonly messageService: MessageService,
    private readonly usersListStore: UserListStore
  ) {
    super(initialState);
  }

  public readonly deleteUser = this.effect<{ userId: string }>((trigger$ => {
    return trigger$.pipe(
      tap(() => this.patchState({ isLoading: true })),
      exhaustMap(({ userId }) => this.manageUsersService.deleteUser(userId).pipe(
        tap((res) => {
          this.showSuccessDeleteToast();
          this.patchState({ isLoading: false })
          this.usersListStore.getUsers();
        }),
        catchError(error => {
          this.showErrorDeleteToast();
          this.patchState({ isLoading: false })
          return of(error)
        })
      ))
    )
  }
  ))

  showSuccessDeleteToast(): void {
    this.messageService.add({
      severity: 'success',
      summary: 'User Deleted',
      detail: 'The user was deleted successfully.'
    });
  }

  showErrorDeleteToast(): void {
    this.messageService.add({
      severity: 'error',
      summary: 'Delete Failed',
      detail: 'Could not delete the user. Please try again.'
    });
  }
}
