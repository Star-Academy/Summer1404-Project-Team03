import { Component, input, output } from '@angular/core';
import {FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { TableService } from '../../services/table.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { DialogModule } from 'primeng/dialog'

@Component({
  selector: 'app-rename-table',
  imports: [ReactiveFormsModule, CommonModule, ButtonModule, MessageModule, DialogModule],
  templateUrl: './rename-table.component.html',
  styleUrl: './rename-table.component.scss'
})
export class RenameTableComponent {
  public visible = input.required<boolean>();
  public tableName = input.required<string>();
  public schemaId = input.required<string>();
  public close = output<void>();

  public exampleForm!: FormGroup;
  public formSubmitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly tableService: TableService,
    private readonly messageService: MessageService
  ) {
    this.exampleForm = this.fb.group({
      name: [this.tableName(), Validators.required],
    });
  }

  public onSubmit() {
    this.formSubmitted = true;
    if (!this.exampleForm.invalid) {
      this.exampleForm.markAllAsTouched();

      this.tableService.renameTable(+this.schemaId(), this.exampleForm.value).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Table name updated successfully',
          });
          this.onClose();
        }
      });
    }
    this.formSubmitted = false;
  }

  public onClose(): void {
    this.close.emit();
  }

  public isInvalid(controlName: string) {
    const control = this.exampleForm.get(controlName);
    return control?.invalid && (control.touched || this.formSubmitted);
  }
}
