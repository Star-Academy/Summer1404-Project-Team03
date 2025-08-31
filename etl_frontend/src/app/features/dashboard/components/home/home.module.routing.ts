import { Routes } from "@angular/router";
import { HomeComponent } from "./home.component";


export const homeRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
        children: [
            {
                path: 'workflows',
                loadComponent: () => import('./manage-workflows/manage-workflows.component').then(m => m.ManageWorkflowsComponent)
            },
            {
                path: 'files',
                loadComponent: () => import('./manage-files/manage-files.component').then(m => m.ManageFilesComponent)
            }
        ]
    },
]