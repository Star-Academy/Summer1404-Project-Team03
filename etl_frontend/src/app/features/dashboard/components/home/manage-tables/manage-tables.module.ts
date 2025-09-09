import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { manageTablesRoutes } from './manage-tables.routing';
import { ManageTablesComponent } from './manage-tables.component';

@NgModule({
  declarations: [ManageTablesComponent],
  imports: [
    CommonModule,
    RouterModule,
    RouterModule.forRoot(manageTablesRoutes)
  ]
})
export class ManageTablesModule { }
