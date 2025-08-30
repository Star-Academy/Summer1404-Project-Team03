import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { UserStoreService } from '../stores/user-store.service';

export const sysAdminGuard: CanMatchFn = (route, segments) => {
  const userStore = inject(UserStoreService);
  const router = inject(Router);

  const isSysAdmin = userStore.vm().user.roles
    .some(role => role.name === "sys_admin");

  if (isSysAdmin) {
    return true;
  }

  router.navigateByUrl('/profile')
  return false;
};
