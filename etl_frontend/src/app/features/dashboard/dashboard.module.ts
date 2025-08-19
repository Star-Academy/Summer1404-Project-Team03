import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Button } from 'primeng/button';
import { Toolbar } from 'primeng/toolbar';
import { Avatar } from 'primeng/avatar';
import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';
import { dashboardRoutes } from './dashboard.module.routing';
import {HeaderComponent} from "../../shared/components/header/header.component";
import {ProfilePopoverComponent} from './components/profile-popover/profile-popover.component';

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    Button,
    Toolbar,
    Avatar,
    RouterModule.forChild(dashboardRoutes),
    HeaderComponent,
    ProfilePopoverComponent
  ],
})
export class DashboardModule { }
