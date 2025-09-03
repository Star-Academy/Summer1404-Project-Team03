import { Component } from '@angular/core';
import { FileUploaderComponent } from './components/file-uploader/file-uploader.component';

@Component({
  selector: 'app-add-new-file',
  templateUrl: './add-new-file.component.html',
  imports: [FileUploaderComponent],
  styleUrl: './add-new-file.component.scss'
})
export class AddNewFileComponent {

}
