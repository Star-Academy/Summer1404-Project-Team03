import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FilesManagementStore } from './stores/files-management/files-management.service';

@Component({
  selector: 'app-manage-files',
  standalone: false,
  templateUrl: './manage-files.component.html',
  styleUrl: './manage-files.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ManageFilesComponent {
  public readonly vm;

  constructor(private readonly manageFilesStore: FilesManagementStore) {
    this.vm = this.manageFilesStore.vm;
    this.manageFilesStore.getFiles();
  }

  onDeleteFile(fileId: number): void {
    this.manageFilesStore.deleteFile({fileId});
  }

  onCreateTable(fileId: number): void {
    this.manageFilesStore.createTable(fileId);
  }

}
