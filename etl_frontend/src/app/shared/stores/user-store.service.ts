import { Injectable } from '@angular/core';
import { ComponentStore } from "@ngrx/component-store";
import { tap } from 'rxjs';
import { UsersService } from "../services/user/users.service"

export type UserStore = {
  user: object
  isLoading: boolean;
}

const initialUser = {user: { }, isLoading: false}

@Injectable({
  providedIn: 'root'
})
export class UserStoreService extends ComponentStore<UserStore> {
  constructor(private readonly http: UsersService) {
    super(initialUser);
  }

  private readonly user$ = this.select((state) => state.user);
  private readonly isLoading$ = this.select((state) => state.isLoading);

  public readonly vm$ = this.select(
    this.user$,
    this.isLoading$,
    (user, isLoading) => ({ user, isLoading })
  )

  readonly setUser = this.updater((state, user: object) => ({ ...state, user }));
  readonly setLoading = this.updater((state, isLoading: boolean) => ({ ...state, isLoading }));

  public loadUser(): void {
    this.setLoading(true);

    this.http.getUserInformation().pipe(
      tap({
        next: (user: object) => {
          this.setUser(user);
          this.setLoading(false);
        },
        error: () => {
          this.setLoading(false);
        }
      })
    ).subscribe();
  }

  public clearUser(): void {
    this.patchState(initialUser);
  }
}
