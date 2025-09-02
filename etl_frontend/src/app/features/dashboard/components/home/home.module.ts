import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { RouterModule } from '@angular/router';
import { homeRoutes } from './home.module.routing';
import { ManageFilesComponent } from './manage-files/manage-files.component';
import { ManageWorkflowsComponent } from './manage-workflows/manage-workflows.component';
import { MenuModule } from 'primeng/menu';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { FileUploadModule } from 'primeng/fileupload';



@NgModule({
  declarations: [HomeComponent, ManageFilesComponent, ManageWorkflowsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(homeRoutes),
    RouterModule,
    MenuModule,
    FormsModule,
    CardModule,
    TableModule,
    ButtonModule,
    FileUploadModule,
    DropdownModule,
  ]
})
export class HomeModule { }
