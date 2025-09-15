import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { profileRoutes } from './profile.module.routing';
import { ProfileComponent } from './profile.component';
import { ButtonModule } from 'primeng/button';
import { TabsModule } from 'primeng/tabs';
import { Button } from 'primeng/button';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { SignOutDirective } from '../../shared/directives/sign-out/sign-out.directive'
import { HasRoleDirective } from '../../shared/directives/has-role/has-role.directive';

@NgModule({
  declarations: [
    ProfileComponent,
  ],
  imports: [
    CommonModule,
    Button,
    RouterModule.forChild(profileRoutes),
    ButtonModule,
    TabsModule,
    RouterModule,
    HeaderComponent,
    SignOutDirective,
    HasRoleDirective
  ],
})
export class ProfileModule { }
