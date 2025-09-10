import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { homeRoutes } from './home.module.routing';
import { MenuModule } from 'primeng/menu';
import { HomeComponent } from './home.component';
import { TableService } from './manage-tables/services/table.service';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    RouterModule,
    MenuModule,
    RouterModule.forChild(homeRoutes)
  ],
  providers: [TableService]
})
export class HomeModule { }
