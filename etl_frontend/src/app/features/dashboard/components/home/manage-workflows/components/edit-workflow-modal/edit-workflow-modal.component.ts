import { Component, effect, input, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { WorkflowPut } from '../../models/workflow.model';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { CommonModule } from '@angular/common';
import { EditWorkflowStore } from './stores/edit-workflow/edit-workflow-store.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-edit-workflow-modal',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    InputTextModule,
    ButtonModule,
    SelectModule,
    FormsModule
  ],
  providers: [EditWorkflowStore],
  templateUrl: './edit-workflow-modal.component.html',
  styleUrl: './edit-workflow-modal.component.scss'
})
export class EditWorkflowModalComponent implements OnInit {
  public visible = input<boolean>(false);
  public workflowId = input.required<string>();
  public close = output<void>();
  public edited = output<void>();
  public readonly vm;

  workflowForm: FormGroup;

  readonly statusOptions = [
    { label: 'Running', value: 'Running' },
    { label: 'Completed', value: 'Completed' },
    { label: 'Failed', value: 'Failed' },
    { label: 'Draft', value: 'Draft' }
  ];

  constructor(
    private fb: FormBuilder,
    private readonly editWorkflowStore: EditWorkflowStore,
    private readonly messageService: MessageService) {
    this.vm = this.editWorkflowStore.vm;

    this.workflowForm = this.fb.group({
      name: [this.vm().workflow?.name, Validators.required],
      description: ['', Validators.required],
      status: ['', Validators.required],
    });

    effect(() => {
      const workflow = this.vm().workflow;

      if (workflow) {
        this.workflowForm.patchValue({
          name: workflow.name,
          description: workflow.description,
          status: workflow.status,
        });
      }
    });
  }

  ngOnInit(): void {
    this.editWorkflowStore.getWorkflow(this.workflowId());

    this.editWorkflowStore.editResult$.subscribe((result) => {
      if (result === 'success') {
        this.messageService.add({
          severity: 'success',
          summary: 'Workflow updated successfully',
        });
        this.edited.emit();
        this.onClose();
      } else {
        this.messageService.add({
          severity: 'error',
          summary: 'Failed to update workflow',
        });
      }
    })
  }

  get f() {
    return this.workflowForm.controls;
  }

  onClose() {
    this.close.emit();
    this.workflowForm.reset();
  }

  onSave() {
    if (this.workflowForm.valid) {
      const editedWorkflow: WorkflowPut = this.workflowForm.value;
      this.editWorkflowStore.editWorkflow({
        workflowId: this.workflowId(),
        editedWorkflow,
      });
    } else {
      this.workflowForm.markAllAsTouched();
    }
  }
}
