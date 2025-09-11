import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule } from '@angular/router';
import { manageTablesRoutes } from './manage-tables.routing';
import { ManageTablesComponent } from './manage-tables.component';
import { CardModule } from 'primeng/card';
import {TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';
import { TableStoreService } from './stores/table-store.service';
import { TableService } from './services/table.service';
import { RenameTableComponent } from './components/rename-table/rename-table.component';

@NgModule({
  declarations: [ManageTablesComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    CardModule,
    TableModule,
    ButtonModule,
    DropdownModule,
    TooltipModule,
    RenameTableComponent,
    RouterModule.forChild(manageTablesRoutes)
  ],
providers:[TableStoreService, ]
})
export class ManageTablesModule { }
