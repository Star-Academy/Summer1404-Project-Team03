import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkflowWorkspaceComponent } from './workflow-workspace.component';
import { RouterModule } from '@angular/router';
import { workflowEditorRoutes } from './workflow-workspace-module.routing';
import { WorkflowSidebarComponent } from './components/workflow-sidebar/workflow-sidebar.component';
import { TableSelectorStore } from './components/workflow-sidebar/workflow-table-selector/stores/table-selector-store.service';



@NgModule({
  declarations: [WorkflowWorkspaceComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(workflowEditorRoutes),
    WorkflowSidebarComponent
  ],
  providers: [TableSelectorStore]
})
export class WorkflowEditorModule { }
