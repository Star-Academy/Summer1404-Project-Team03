import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Button } from 'primeng/button';
import { Toolbar } from 'primeng/toolbar';
import { Avatar } from 'primeng/avatar';
import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';
import { dashboardRoutes } from './dashboard.module.routing';



@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    Button,
    Toolbar,
    Avatar,
    RouterModule.forChild(dashboardRoutes)
  ],
})
export class DashboardModule { }
