import { Component, input, OnInit, output, WritableSignal } from '@angular/core';
import {FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { TableColumnService } from '../../services/table-column.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { DialogModule } from 'primeng/dialog'

@Component({
  selector: 'app-rename-column',
  imports: [CommonModule, MessageModule, ButtonModule, DialogModule, ReactiveFormsModule],
  templateUrl: './rename-column.component.html',
  styleUrl: './rename-column.component.scss'
})

export class RenameColumnComponent implements OnInit {
  public visible = input.required<boolean>();
  public columnName = input.required<string>();
  public columnId = input.required<string>();
  public close = output<void>();
  private schemaId!: WritableSignal<string> ;

  public exampleForm!: FormGroup;
  public formSubmitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly columnService: TableColumnService,
    private readonly messageService: MessageService
  ) {
    this.exampleForm = this.fb.group({
      name: [this.columnName(), Validators.required],
    });
  }

  public onSubmit() {
    this.formSubmitted = true;
    if (!this.exampleForm.invalid) {
      this.exampleForm.markAllAsTouched();

      this.columnService.renameTableColumn(+this.schemaId(), +this.columnId(), this.exampleForm.value ).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Column name updated successfully',
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

  ngOnInit() {
    this.schemaId.set(this.route.snapshot.paramMap.get('table-id')!)
  }
}
