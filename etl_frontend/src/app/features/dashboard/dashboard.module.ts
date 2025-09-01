import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Button, ButtonModule } from 'primeng/button';
import { Toolbar } from 'primeng/toolbar';
import { Avatar } from 'primeng/avatar';
import { DashboardComponent } from './dashboard.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { RouterModule } from '@angular/router';
import { dashboardRoutes } from './dashboard.module.routing';
import { HeaderComponent } from "../../shared/components/header/header.component";
import { ProfilePopoverComponent } from './components/profile-popover/profile-popover.component';
import { WorkflowsListStore } from './stores/workflows-list/workflows-list-store.service';
import { WorkflowsTabsManagementComponent } from './components/workflows-tabs-management/workflows-tabs-management.component';
import { WorkflowTabComponent } from './components/workflows-tabs-management/components/workflow-tab/workflow-tab.component';
import { TooltipModule } from 'primeng/tooltip';
import { SkeletonModule } from 'primeng/skeleton';
import { WorkflowSelectorComponent } from './components/workflow-selector/workflow-selector.component';
import { PopoverModule } from 'primeng/popover';
import { ListboxModule } from 'primeng/listbox';
import { ButtonGroupModule } from 'primeng/buttongroup';
import { InputTextModule } from 'primeng/inputtext';

@NgModule({
  declarations: [DashboardComponent, WorkflowsTabsManagementComponent, WorkflowTabComponent, WorkflowSelectorComponent],
  imports: [
    CommonModule,
    Button,
    Toolbar,
    Avatar,
    RouterModule.forChild(dashboardRoutes),
    HeaderComponent,
    ProfilePopoverComponent,
    TooltipModule,
    DragDropModule,
    SkeletonModule,
    PopoverModule,
    ListboxModule,
    ButtonGroupModule,
    InputTextModule
  ],
  providers: [WorkflowsListStore]
})
export class DashboardModule { }
