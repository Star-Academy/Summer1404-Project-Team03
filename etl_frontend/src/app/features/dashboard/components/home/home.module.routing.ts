import { Routes } from "@angular/router";
import { HomeComponent } from "./home.component";
import { ManageWorkflowsComponent } from "./manage-workflows/manage-workflows.component";
import { ManageFilesComponent } from "./manage-files/manage-files.component";


export const homeRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'workflows'
            },
            {
                path: 'workflows',
                component: ManageWorkflowsComponent
                // loadComponent: () => import('./manage-workflows/manage-workflows.component').then(m => m.ManageWorkflowsComponent)
            },
            {
                path: 'files',
                component: ManageFilesComponent,
                // loadComponent: () => import('./manage-files/manage-files.component').then(m => m.ManageFilesComponent)
            }
        ]
    },
]