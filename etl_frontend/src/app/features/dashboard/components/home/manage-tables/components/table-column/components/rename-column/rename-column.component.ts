import { Component, input, OnInit, output, signal, WritableSignal } from '@angular/core';
import {FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { TableColumnService } from '../../services/table-column.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, NgClass } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { DialogModule } from 'primeng/dialog'
import { TableColumnStoreService } from '../../stores/table-column-store.service';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-rename-column',
  imports: [CommonModule, MessageModule, ButtonModule, DialogModule, ReactiveFormsModule, NgClass, InputTextModule],
  templateUrl: './rename-column.component.html',
  styleUrl: './rename-column.component.scss'
})

export class RenameColumnComponent implements OnInit {
  public visible = input.required<boolean>();
  public name = input.required<string>();
  public columnId = input.required<number>();
  public close = output<void>();
  private schemaId= signal<number>(0);

  public exampleForm!: FormGroup;
  public formSubmitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly activatRoute: ActivatedRoute,
    private readonly router: Router,
    private readonly columnService: TableColumnService,
    private readonly columnStore: TableColumnStoreService,
    private readonly messageService: MessageService
  ) {}

  public onSubmit() {
    this.formSubmitted = true;
    if (!this.exampleForm.invalid) {
      this.exampleForm.markAllAsTouched();

      this.columnService.renameTableColumn(this.schemaId(), this.columnId(), {newName: this.exampleForm.value.name} ).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Column name updated successfully',
          });
          this.columnStore.loadColumn(this.schemaId())
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
    this.exampleForm = this.fb.group({
      name: [this.name(), Validators.required],
    });
    this.activatRoute.params.subscribe((params) => {
      const tableId = params['table-id'];
      if (tableId) {
        this.schemaId.set(tableId);
      }
      else this.router.navigateByUrl('/dashboard/tables/list');
    });
  }
}
