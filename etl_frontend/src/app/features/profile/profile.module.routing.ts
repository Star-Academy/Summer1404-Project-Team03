import {Routes} from "@angular/router";
import {ProfileComponent} from "./profile.component";
import { ManageUserComponent } from "./components/manage-user/manage-user.component";
import { sysAdminGuard } from "../../shared/guards/sys-admin.guard";

export const profileRoutes: Routes = [
  {
    path: '',
    component: ProfileComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'detail'
      },
      {
        path: '',
        loadComponent: () => import('./components/profile-detail/profile-detail.component').then(c => c.ProfileDetailComponent)
      },
      {
        path: 'admin',
        loadChildren: () => import('./components/manage-user/manage-user.module').then(c => c.ManageUsersModule),
        canMatch: [sysAdminGuard]
      }
    ]
  }
]
