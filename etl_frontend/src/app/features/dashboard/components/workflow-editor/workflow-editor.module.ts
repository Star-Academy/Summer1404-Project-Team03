import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkflowEditorComponent } from './workflow-editor.component';
import { RouterModule } from '@angular/router';
import { workflowEditorRoutes } from './workflow-editor-module.routing';



@NgModule({
  declarations: [WorkflowEditorComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(workflowEditorRoutes)
  ]
})
export class WorkflowEditorModule { }
