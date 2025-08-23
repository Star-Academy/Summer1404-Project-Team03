import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { profileRoutes } from './profile.module.routing';
import { HeaderComponent } from '../../../../shared/components/header/header.component';
import { ProfileComponent } from './profile.component';
import { ButtonModule } from 'primeng/button';
import {TabsModule} from "primeng/tabs";
import { MessageService } from 'primeng/api';


@NgModule({
  declarations: [
    ProfileComponent
  ],
  imports: [
    CommonModule,
    HeaderComponent,
    RouterModule.forChild(profileRoutes),
    ButtonModule,
    TabsModule,
    RouterModule,
  ],
  providers: [
    MessageService,
  ]
})
export class ProfileModule { }
