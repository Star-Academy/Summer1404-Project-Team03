import { Routes } from "@angular/router";
import { ManageWorkflowsComponent } from "./manage-workflows.component";


export const manageWorkflowRoutes: Routes = [
    {
        path: '',
        component: ManageWorkflowsComponent,
    },
    // {
    //     path: 'new-file',
    //     children: [
    //         {
    //             // path: ':file-name',
    //         }
    //     ]
    // }
]
