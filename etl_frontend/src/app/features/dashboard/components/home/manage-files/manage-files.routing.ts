import { Routes } from "@angular/router";
import { ManageFilesComponent } from "./manage-files.component";
import { AddNewFileComponent } from "./components/add-new-file/add-new-file.component";


export const manageFilesRoutes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
    },
    {
        path: 'list',
        component: ManageFilesComponent,
    },
    {
        path: 'new-file',
        component: AddNewFileComponent
    }
]