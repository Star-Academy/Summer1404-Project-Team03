import { Routes } from "@angular/router";
import { ProfileComponent } from "./profile.component";
import { ProfileDetailComponent } from "./components/profile-detail/profile-detail.component";
import { ManageUserComponent } from "./components/manage-user/manage-user.component";


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
                path: 'detail',
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
                path: 'manage-user',
                loadComponent: () => import('./components/manage-user/manage-user.component').then(c => c.ManageUserComponent)
            }
        ]
    }
]
