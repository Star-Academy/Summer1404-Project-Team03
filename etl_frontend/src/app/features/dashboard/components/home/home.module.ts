import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeComponent} from './home.component';
import {RouterModule} from '@angular/router';
import {homeRoutes} from './home.module.routing';
import {MenuModule} from 'primeng/menu';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(homeRoutes),
    RouterModule,
    MenuModule,
  ]
})
export class HomeModule {
}
