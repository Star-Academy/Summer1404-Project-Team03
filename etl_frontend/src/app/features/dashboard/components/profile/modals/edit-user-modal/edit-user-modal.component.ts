import { Component, input, output } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { Button } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-edit-user-modal',
  imports: [Button, Dialog, DropdownModule, InputTextModule],
  templateUrl: './edit-user-modal.component.html',
  styleUrls: ['./edit-user-modal.component.scss', '../shared/shared-modal.component.scss']
})
export class EditUserModalComponent {
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
