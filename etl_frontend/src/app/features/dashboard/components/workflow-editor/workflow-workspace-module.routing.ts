import { Routes } from "@angular/router";
import { WorkflowWorkspaceComponent } from "./workflow-workspace.component";


export const workflowEditorRoutes: Routes = [
    {
        path: ':workflow-id',
        component: WorkflowWorkspaceComponent
    }
]