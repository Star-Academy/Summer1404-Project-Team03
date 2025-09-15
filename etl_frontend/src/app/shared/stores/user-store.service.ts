import { computed, Injectable } from '@angular/core';
import { ComponentStore } from "@ngrx/component-store";
import { finalize, tap } from 'rxjs';
import { UsersService } from "../services/user/users.service"
import { UserInfo } from '../models/user.model';

type UserStore = {
  user: UserInfo;
  isLoading: boolean;
  isSysAdmin: boolean;
}

const initialUser = {
  user: {
    email: '',
    firstName: '',
    id: '',
    lastName: '',
    roles: [],
    name: '',
  },
  isLoading: false,
  isSysAdmin: false,
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

  private readonly isSysAdmin = this.selectSignal((state) => state.isSysAdmin);

  public readonly hasAnyRole = (roles: string[]) =>
    computed(() => this.user().roles.some(r => roles.includes(r.name)));

  private readonly setUser = this.updater((state, user: UserInfo) => ({ ...state, user }));

  private readonly setLoading = this.updater((state, value: boolean) => ({
    ...state,
    isLoading: value
  }));

  private readonly setSysAdmin = this.updater((state, value: boolean) => ({
    ...state,
    isSysAdmin: value
  }));

  public readonly vm = this.selectSignal(
    this.user,
    this.isLoading,
    this.isSysAdmin,
    (user, isLoading, isSysAdmin) => ({ user, isLoading, isSysAdmin })
  );

  public readonly vm$ = this.select(s => s);

  public loadUser(): void {
    this.setLoading(true);

    this.http.getUserInformation().pipe(
      tap({
        next: (user: UserInfo) => {
          const isSysAdmin = user.roles.some(role => role.name === 'sys_admin')
          this.setUser(user)
          this.setSysAdmin(isSysAdmin)
        },
        error: () => {
          this.setUser(initialUser.user);
          this.setSysAdmin(false);
        }
      }),
      finalize(() => this.setLoading(false)),
    ).subscribe();
  }

  public clearUser(): void {
    this.patchState(initialUser);
  }
}
