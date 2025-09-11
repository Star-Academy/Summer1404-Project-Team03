import { Routes } from "@angular/router";
import { ManageTablesComponent } from "./manage-tables.component";
import { TableColumnComponent } from "./components/table-column/table-column.component";



export const manageTablesRoutes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
    },
    {
        path: 'list',
        component: ManageTablesComponent,
        children: [
          {
            path: ':table-id',
            loadComponent: () =>
              import('./components/table-column/table-column.component').then(c => c.TableColumnComponent)
          }
        ]
    },
]
