import { Component } from '@angular/core';
import { UploadFileStore } from './stores/upload-file/upload-file-store.service';
import { RouterOutlet } from '@angular/router';
import { FileProcessStepperComponent } from '../../components/add-new-file/components/file-process-stepper/file-process-stepper.component'

@Component({
  selector: 'app-add-new-file',
  templateUrl: './add-new-file.component.html',
  imports: [FileProcessStepperComponent, RouterOutlet],
  providers: [UploadFileStore],
  styleUrl: './add-new-file.component.scss',
})
export class AddNewFileComponent {}
