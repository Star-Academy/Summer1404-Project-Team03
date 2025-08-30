import {Component, input, output} from '@angular/core';
import { Dialog } from "primeng/dialog";
import { Button } from "primeng/button";
import { DeleteUserStore } from './stores/delete-user/delete-user-store.service';

@Component({
  selector: 'app-delete-user-dialog',
  imports: [Dialog, Button],
  templateUrl: './delete-user-dialog.component.html',
  styleUrl: './delete-user-dialog.component.scss'
})
export class DeleteUserDialogComponent {
  public readonly visible = input.required<boolean>();
  public close = output<void>();
  userId = input.required<string>();

  constructor(private readonly deleteUserStore: DeleteUserStore) {}

  onDelete(): void {
    this.deleteUserStore.deleteUser({userId: this.userId()});
    this.close.emit();
  }

  public onClose() {
    this.close.emit();
  }
}
