import {Routes} from "@angular/router";
import {ProfileComponent} from "./profile.component";

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
        path: 'edit',
        loadComponent: () => import('./components/edit-profile/edit-profile.component').then(c => c.EditProfileComponent)
      },
      {
        path: 'change-password',
        loadComponent: () => import('./components/change-password/change-password.component').then(c => c.ChangePasswordComponent)
      },
      {
        path: 'admin',
        loadChildren: () => import('./components/manage-user/manage-user.module').then(c => c.ManageUsersModule)
      }
    ]
  }
]
