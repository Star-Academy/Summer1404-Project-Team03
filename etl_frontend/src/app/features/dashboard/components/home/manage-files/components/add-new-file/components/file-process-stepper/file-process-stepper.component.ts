import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { StepperModule } from 'primeng/stepper';
import { SchemaEditorComponent } from '../../../schema-editor/schema-editor.component';
import { ActivatedRoute } from '@angular/router';
import { UploadFileStore } from '../../stores/upload-file/upload-file-store.service';

@Component({
  selector: 'app-file-process-stepper',
  imports: [StepperModule, ButtonModule, SchemaEditorComponent],
  templateUrl: './file-process-stepper.component.html',
  styleUrl: './file-process-stepper.component.scss',
})
export class FileProcessStepperComponent {
  public activeStep = 1;
  public fileName: string | null = null;
  public readonly vm;

  constructor(
    private route: ActivatedRoute,
    public uploadFileStore: UploadFileStore
  ) {
    this.route.paramMap.subscribe(params => {
      this.fileName = params.get('file-name');
    })
    this.vm = this.uploadFileStore.vm;
  }

  onUpload(activateCallback: (val: number) => void) {
    const fileName = this.fileName;
    console.log(fileName);
    if (!fileName) return;
    this.uploadFileStore.uploadResult$.subscribe(res => {
      if (res === 'success') {
        activateCallback(2);
      }
    })
    this.uploadFileStore.uploadFileWithName({ fileName });
  }

  onSchemaEdited(config: any) {
    console.log('Schema configured:', config);
    // You can store schema in the store or service here
  }

  onCreateTable() {
    console.log(`Creating table for ${this.fileName}`);
    // Call your backend API or store effect
  }
}
