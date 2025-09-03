import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ConfirmationService, MessageService} from 'primeng/api';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
  ],
  providers: [MessageService, ConfirmationService]
})
export class CoreModule {
}
