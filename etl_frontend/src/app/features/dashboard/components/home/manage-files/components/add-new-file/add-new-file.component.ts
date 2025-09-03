import { Component } from '@angular/core';
import { FileUploaderComponent } from './components/file-uploader/file-uploader.component';
import { UploadFileStore } from './stores/upload-file/upload-file-store.service';

@Component({
  selector: 'app-add-new-file',
  templateUrl: './add-new-file.component.html',
  imports: [FileUploaderComponent],
  providers: [UploadFileStore],
  styleUrl: './add-new-file.component.scss',
})
export class AddNewFileComponent {}
