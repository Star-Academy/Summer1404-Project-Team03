import {Routes} from "@angular/router";
import {ManageTablesComponent} from "./manage-tables.component";


export const manageTablesRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'list'
  },
  {
    path: 'list',
    component: ManageTablesComponent,
  },
  {
    path: ':table-id',
    loadComponent: () =>
      import('./components/table-column/table-column.component').then(c => c.TableColumnComponent)
  }
]
