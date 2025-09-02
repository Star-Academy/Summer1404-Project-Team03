import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageFilesComponent } from './manage-files.component';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { FileUploadModule } from 'primeng/fileupload';
import { DropdownModule } from 'primeng/dropdown';
import { RouterModule } from '@angular/router';
import { manageFilesRoutes } from './manage-files.routing';



@NgModule({
  declarations: [ManageFilesComponent],
  imports: [
    CommonModule,
    FormsModule,
    CardModule,
    TableModule,
    ButtonModule,
    FileUploadModule,
    DropdownModule,
    RouterModule,
    RouterModule.forChild(manageFilesRoutes)
  ]
})
export class ManageFilesModule { }
