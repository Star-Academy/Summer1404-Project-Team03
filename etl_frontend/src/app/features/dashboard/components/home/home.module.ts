import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { RouterModule } from '@angular/router';
import { homeRoutes } from './home.module.routing';
import { ManageFilesComponent } from './manage-files/manage-files.component';
import { manageUserRoutes } from '../../../profile/components/manage-user/manage-user.module.routing';
import { ManageWorkflowsComponent } from './manage-workflows/manage-workflows.component';



@NgModule({
  declarations: [HomeComponent, ManageFilesComponent, ManageWorkflowsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(homeRoutes),
    RouterModule,
  ]
})
export class HomeModule { }
