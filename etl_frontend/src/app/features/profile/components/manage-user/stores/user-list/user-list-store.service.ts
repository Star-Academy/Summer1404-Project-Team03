import { Injectable } from '@angular/core';
import { ComponentStore, } from '@ngrx/component-store';
import { User, UsersListState } from '../../models/user.model';
import { catchError, exhaustMap, of, tap } from 'rxjs';
import { ManageUsersService } from '../../services/mange-users/manage-users.service';

const initialUser: UsersListState = {
  users: [],
  isLoading: false,
  error: null
}

@Injectable()
export class UserListStore extends ComponentStore<UsersListState> {

  constructor(
    private readonly mangeUsersService: ManageUsersService,
  ) {
    super(initialUser);
  }

  public readonly vm = this.selectSignal(s => s);

  public readonly getUsers = this.effect<void>(trigger$ => {
    return trigger$.pipe(
      tap(() => this.patchState({ isLoading: true, error: null })),
      exhaustMap(() => this.mangeUsersService.fetchUsers().pipe(
        tap((users) => this.patchState({ users: users, isLoading: false })),
        catchError(err => {
          this.patchState({ error: err.message, isLoading: false });
          return of(err);
        })
      ))
    )
  })
}
