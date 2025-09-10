import { Component, signal } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { StepperModule } from 'primeng/stepper';
import { SchemaEditorComponent } from '../../../schema-editor/schema-editor.component';
import { ActivatedRoute } from '@angular/router';
import { UploadFileStore } from '../../stores/upload-file/upload-file-store.service';
import { FileItem } from '../../../../models/file.model';
import { TableService } from '../../../../../manage-tables/services/table.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-file-process-stepper',
  imports: [StepperModule, ButtonModule, SchemaEditorComponent],
  templateUrl: './file-process-stepper.component.html',
  styleUrl: './file-process-stepper.component.scss',
})
export class FileProcessStepperComponent {
  public activeStep = 1;
  public fileName: string | null = null;
  public fileId = signal<number | undefined>(undefined);
  stepperActivateCallback: ((step: number) => void) | undefined = undefined;
  public readonly vm;

  constructor(
    private route: ActivatedRoute,
    public uploadFileStore: UploadFileStore,
    private readonly tableService: TableService,
    private readonly messageService: MessageService
  ) {
    this.route.paramMap.subscribe((params) => {
      this.fileName = params.get('file-name');
    });
    this.vm = this.uploadFileStore.vm;
    this.uploadFileStore.uploadResult$.subscribe((res) => {
      if (Array.isArray(res) && res.length > 0) {
        this.fileId.set(res[0].id);
        this.stepperActivateCallback?.(2);
      }
    });
  }

  onUpload(activateCallback: (val: number) => void) {
    this.stepperActivateCallback = activateCallback;
    const fileName = this.fileName;
    if (!fileName) return;
    this.uploadFileStore.uploadFileWithName({ fileName });
  }

  onSchemaEdited(config: any) {
    this.stepperActivateCallback?.(3);
  }

  onCreateTable() {
    const fileId = this.fileId();
    if (fileId)
      this.tableService.createTable(fileId).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Table created successfully',
          });
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Table creation failed',
          });
        },
      });
  }
}
