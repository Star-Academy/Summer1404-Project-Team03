import { Routes } from "@angular/router";
import { WorkflowWorkspaceComponent } from "./workflow-workspace.component";
import { WorkflowCanvasComponent } from "./components/workflow-canvas/workflow-canvas.component";


export const workflowEditorRoutes: Routes = [
    {
        path: '',
        component: WorkflowWorkspaceComponent,
        children: [
            {
                path: ':workflow-id',
                loadComponent: () => import('./components/workflow-canvas/workflow-canvas.component').then(c => c.WorkflowCanvasComponent)
            }
        ]
    }
]