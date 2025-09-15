import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkflowWorkspaceComponent } from './workflow-workspace.component';
import { RouterModule } from '@angular/router';
import { workflowEditorRoutes } from './workflow-workspace-module.routing';
import { WorkflowSidebarComponent } from './components/workflow-sidebar/workflow-sidebar.component';



@NgModule({
  declarations: [WorkflowWorkspaceComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(workflowEditorRoutes),
    WorkflowSidebarComponent
  ]
})
export class WorkflowEditorModule { }
