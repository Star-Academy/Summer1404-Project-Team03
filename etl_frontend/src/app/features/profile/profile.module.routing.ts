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
        path: 'admin',
        loadComponent: () => import('./components/manage-user/manage-user.component').then(c => c.ManageUserComponent)
      }
    ]
  }
]
