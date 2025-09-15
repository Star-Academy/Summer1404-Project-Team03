import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkflowWorkspaceComponent } from './workflow-workspace.component';
import { RouterModule } from '@angular/router';
import { workflowEditorRoutes } from './workflow-workspace-module.routing';



@NgModule({
  declarations: [WorkflowWorkspaceComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(workflowEditorRoutes)
  ]
})
export class WorkflowEditorModule { }
