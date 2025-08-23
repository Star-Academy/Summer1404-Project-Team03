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
                path: 'detail',
                component: ProfileDetailComponent
            },
            {
                path: 'manage-user',
                component: ManageUserComponent
            }
        ]
    }
]
