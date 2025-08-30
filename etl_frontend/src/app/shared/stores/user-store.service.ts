import { Injectable } from '@angular/core';
import { ComponentStore } from "@ngrx/component-store";
import { finalize, tap } from 'rxjs';
import { UsersService } from "../services/user/users.service"
import { UserInfo } from '../types/UserType';

type UserStore = {
  user: UserInfo;
  isLoading: boolean;
}

const initialUser = {
  user: {
    email: '',
    firstName: '',
    id: '',
    lastName: '',
    roles: [],
    username: '',
  }, isLoading: false
}

@Injectable({
  providedIn: 'root'
})
export class UserStoreService extends ComponentStore<UserStore> {
  constructor(private readonly http: UsersService) {
    super(initialUser);
  }

  private readonly user = this.selectSignal((state) => state.user);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);

  private readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  }));

  public readonly vm = this.selectSignal(
    this.user,
    this.isLoading,
    (user, isLoading) => ({ user, isLoading })
  );

  readonly setUser = this.updater((state, user: UserInfo) => ({ ...state, user }));

  public loadUser(): void {
    this.setLoading(true);

    this.http.getUserInformation().pipe(
      tap({
        next: (user: UserInfo) => this.setUser(user),
      }),
      finalize(() => this.setLoading(false)),
    ).subscribe();
  }

  public clearUser(): void {
    this.patchState(initialUser);
  }
}
