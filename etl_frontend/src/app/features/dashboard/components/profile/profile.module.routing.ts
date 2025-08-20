import { Routes } from "@angular/router";
import { ProfileComponent } from "./profile.component";
import { ProfileDetailComponent } from "./components/profile-detail/profile-detail.component";


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
                component: ProfileDetailComponent
            }
        ]
    }
]