import { Routes } from "@angular/router";
import { HomeComponent } from "./home.component";
import { ManageWorkflowsComponent } from "./manage-workflows/manage-workflows.component";

export const homeRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'workflows',
            },
            {
                path: 'workflows',
                loadChildren: () =>
                  import('./manage-workflows/manage-workflows.module').then(
                    (m) => m.ManageWorkflowsModule
                  )
            },
            {
                path: 'files',
                loadChildren: () =>
                    import('./manage-files/manage-files.module').then(
                        (m) => m.ManageFilesModule
                    ),
            },
        ],
    },
];
