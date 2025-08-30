import { NgModule } from '@angular/core'
import { ManageUserComponent } from './manage-user.component';

import { TooltipModule } from 'primeng/tooltip';
import { ManageUsersService } from './services/mange-users/manage-users.service';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table'
import { CommonModule } from '@angular/common';
import { SkeletonModule } from "primeng/skeleton";
import { InputTextModule } from 'primeng/inputtext';
import { ChipModule } from 'primeng/chip';
import { RippleModule } from 'primeng/ripple';
import { PaginatorModule } from 'primeng/paginator';
import { FormsModule } from '@angular/forms';

import { CreateUserModalComponent } from './components/create-user-modal/create-user-modal.component';
import { EditUserModalComponent } from './components/edit-user-modal/edit-user-modal.component';
import { DeleteUserDialogComponent } from './components/delete-user-dialog/delete-user-dialog.component';
import { UserListStore } from './stores/user-list/user-list-store.service';
import { RouterModule } from '@angular/router';
import { manageUserRoutes } from './manage-user.module.routing';
import { DeleteUserStore } from './components/delete-user-dialog/stores/delete-user/delete-user-store.service';
import { EditUserStore } from './components/edit-user-modal/stores/edit-user/edit-user-store.service';

@NgModule({
    declarations: [ManageUserComponent],
    imports: [
        ButtonModule,
        TableModule,
        CommonModule,
        SkeletonModule,
        InputTextModule,
        RippleModule,
        PaginatorModule,
        FormsModule,
        CreateUserModalComponent,
        EditUserModalComponent,
        DeleteUserDialogComponent,
        ChipModule,
        TooltipModule,
        RouterModule.forChild(manageUserRoutes),
        RouterModule
    ],
    providers: [
        ManageUsersService,
        UserListStore,
        DeleteUserStore,
        EditUserStore
    ],
})
export class ManageUsersModule {

}