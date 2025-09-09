import { Routes } from "@angular/router";
import { ManageTablesComponent } from "./manage-tables.component";



export const manageTablesRoutes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
    },
    {
        path: 'list',
        component: ManageTablesComponent,
    },
    // {
    //     path: 'new-file',
    //     component: AddNewFileComponent,
    //     children: [
    //         {
    //             path: ':file-name/process',
    //             component: FileProcessStepperComponent
    //         }
    //     ]
    // },
    // {
    //     path: ':file-id/edit-schema',
    //     component: SchemaEditorComponent
    // }
]
