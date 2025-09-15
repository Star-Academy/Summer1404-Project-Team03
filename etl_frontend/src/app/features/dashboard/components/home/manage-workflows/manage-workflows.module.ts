import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ManageWorkflowsComponent } from './manage-workflows.component';
import { RouterModule } from '@angular/router';
import { manageWorkflowRoutes } from './manage-workflows.routing';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { TooltipModule } from 'primeng/tooltip';

@NgModule({
  declarations: [ManageWorkflowsComponent],
  imports: [
    CommonModule,
    CardModule,
    RouterModule,
    TableModule,
    ButtonModule,
    DropdownModule,
    FormsModule,
    TooltipModule,
    RouterModule.forChild(manageWorkflowRoutes)
  ]
})
export class ManageWorkflowsModule { }
