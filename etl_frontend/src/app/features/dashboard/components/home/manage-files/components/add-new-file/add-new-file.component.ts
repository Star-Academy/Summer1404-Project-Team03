import { Component } from '@angular/core';
import { FileUploaderComponent } from './components/file-uploader/file-uploader.component';
import { UploadFileStore } from './stores/upload-file/upload-file-store.service';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-add-new-file',
  templateUrl: './add-new-file.component.html',
  imports: [FileUploaderComponent, RouterOutlet],
  providers: [UploadFileStore],
  styleUrl: './add-new-file.component.scss',
})
export class AddNewFileComponent {}
