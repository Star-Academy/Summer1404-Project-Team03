import { Routes } from "@angular/router";
import { HomeComponent } from "./home.component";
import { sysAdminGuard } from "../../../../shared/guards/sys-admin.guard";

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
        path: 'workflows-list',
        loadChildren: () =>
          import('./manage-workflows/manage-workflows.module').then(
            (m) => m.ManageWorkflowsModule
          )
      },
      {
        path: 'files',
        canMatch: [sysAdminGuard],
        data: { roles: ["data_admin", "sys_admin"] },
        loadChildren: () =>
          import('./manage-files/manage-files.module').then(
            (m) => m.ManageFilesModule
          ),
      },
      {
        path: 'tables',
        canMatch: [sysAdminGuard],
        data: { roles: ["data_admin", "sys_admin"] },
        loadChildren: () =>
          import('./manage-tables/manage-tables.module').then(
            (m) => m.ManageTablesModule
          ),
      },
      {
        path: '**',
        redirectTo: 'workflows-list'
      }
    ],
  },
]
