import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { UserStoreService } from '../stores/user-store.service';

export const sysAdminGuard: CanMatchFn = (route, segments) => {
  const userStore = inject(UserStoreService);
  const router = inject(Router);

  let isHavePremision: boolean;
  const neededRoles: string[] = route?.data?.['roles']

  if (neededRoles) {
    isHavePremision = userStore.vm().user.roles.some(role => neededRoles.some((nRole) => role.name === nRole));
    if (isHavePremision) {
      return true;
    }
  }

  return false;
};
