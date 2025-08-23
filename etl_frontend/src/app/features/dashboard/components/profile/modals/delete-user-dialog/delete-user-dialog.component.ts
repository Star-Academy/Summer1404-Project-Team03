import {Component, input, output} from '@angular/core';
import { Dialog } from "primeng/dialog";
import { Button } from "primeng/button";

@Component({
  selector: 'app-delete-user-dialog',
  imports: [Dialog, Button],
  templateUrl: './delete-user-dialog.component.html',
  styleUrl: './delete-user-dialog.component.scss'
})
export class DeleteUserDialogComponent {
  public readonly visible = input.required<boolean>();
  public close = output<void>();

  public onClose() {
    this.close.emit();
  }
}
