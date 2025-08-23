import { Component, input, output } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { Button } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';


@Component({
  selector: 'app-create-user-modal',
  imports: [Dialog, Button, DropdownModule, InputTextModule],
  templateUrl: './create-user-modal.component.html',
  styleUrls: ['./create-user-modal.component.scss', '../shared/shared-modal.component.scss']
})
export class CreateUserModalComponent {
  public visible = input.required<boolean>();
  public close = output<void>();

  public readonly roles = [
    { label: 'Data Admin', value: 'data_admin' },
    { label: 'Data Analyst', value: 'data_analyst' }
  ];

  public onClose() {
    this.close.emit();
  }
}
