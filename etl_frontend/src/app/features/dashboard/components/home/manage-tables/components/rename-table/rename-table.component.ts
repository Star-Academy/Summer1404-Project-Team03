import { Component, input, OnInit, output } from '@angular/core';
import {FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { TableService } from '../../services/table.service';
import { CommonModule, NgClass } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { DialogModule } from 'primeng/dialog'
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { TableStoreService } from '../../stores/table-store.service';

@Component({
  selector: 'app-rename-table',
  imports: [ReactiveFormsModule, CommonModule, ButtonModule, MessageModule, DialogModule, InputTextModule, PasswordModule, NgClass],
  templateUrl: './rename-table.component.html',
  styleUrl: './rename-table.component.scss'
})
export class RenameTableComponent implements OnInit{
  public visible = input.required<boolean>();
  public name = input.required<string>();
  public id = input.required<number>();
  public close = output<void>();

  public exampleForm!: FormGroup;
  public formSubmitted = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly tableService: TableService,
    private readonly tableStore: TableStoreService,
    private readonly messageService: MessageService
  ) {}

  public onSubmit() {
    this.formSubmitted = true;
    if (!this.exampleForm.invalid) {
      this.exampleForm.markAllAsTouched();
      console.log(this.exampleForm.value)

      this.tableService.renameTable(+this.id(), {newTableName: this.exampleForm.value.name}).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Table name updated successfully',
          });
          this.tableStore.loadTables();
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

  ngOnInit(): void {
    this.exampleForm = this.fb.group({
      name: [this.name(), Validators.required],
    });
  }
}
