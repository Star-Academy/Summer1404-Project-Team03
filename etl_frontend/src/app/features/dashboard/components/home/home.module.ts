import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { RouterModule } from '@angular/router';
import { homeRoutes } from './home.module.routing';
import { MenuModule } from 'primeng/menu';
import { TablesManagementService } from './manage-tables/services/tables-management/tables-management.service';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(homeRoutes),
    RouterModule,
    MenuModule,
  ],
  providers: [TablesManagementService]
})
export class HomeModule {
}
