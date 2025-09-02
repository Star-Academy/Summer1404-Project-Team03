import { Routes } from "@angular/router";
import { ManageFilesComponent } from "./manage-files.component";
import { UploadFileComponent } from "./upload-file/upload-file.component";


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
        path: 'upload-file',
        component: UploadFileComponent
    }
]