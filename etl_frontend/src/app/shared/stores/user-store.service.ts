import { Injectable } from '@angular/core';
import { ComponentStore } from "@ngrx/component-store";
import { finalize, tap } from 'rxjs';
import { UsersService } from "../services/user/users.service";

export type User = {
  username: string;
  email: string;
}

type UserStore = {
  user: User;
  isLoading: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserStoreService extends ComponentStore<UserStore> {
  constructor(private readonly http: UsersService) {
    super({ user: { username: '', email: '' }, isLoading: false }); // fixed
  }

  private readonly user = this.selectSignal((state) => state.user);
  private readonly isLoading = this.selectSignal((state) => state.isLoading);

  public readonly vm = this.selectSignal(
    this.user,
    this.isLoading,
    (user, isLoading) => ({ user, isLoading })
  );

  readonly setUser = this.updater((state, user: User) => ({ ...state, user }));

  public loadUser(): void {
    this.patchState({ isLoading: true });

    this.http.getUserInformation().pipe(
      tap({
        next: (user: User) => {
          this.setUser(user)
        },
      }),
      finalize(() => this.patchState({ isLoading: false }))
    ).subscribe();
  }
}
