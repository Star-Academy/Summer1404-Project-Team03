import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { filter, map, take } from 'rxjs';
import { UserStoreService } from '../../stores/user-store.service';


export const authGuard: CanMatchFn = () => {
  const userStore = inject(UserStoreService);
  const router = inject(Router);
  const vm = userStore.vm;

  if (vm().user.id) {
    return true;
  }

  userStore.loadUser();

  return userStore.vm$.pipe(
    filter(({ isLoading }) => !isLoading),
    take(1),
    map(({ user }) => {
      if (user.id) {
        return true;
      }
      return router.createUrlTree(['/landing']);
    })
  );
};