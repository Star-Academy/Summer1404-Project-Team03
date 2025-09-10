import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FilesManagementStore } from './stores/files-management/files-management.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-manage-files',
  standalone: false,
  templateUrl: './manage-files.component.html',
  styleUrl: './manage-files.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ManageFilesComponent {
  public readonly vm;

  constructor(
    private readonly manageFilesStore: FilesManagementStore,
    private readonly messageService: MessageService) {
    this.vm = this.manageFilesStore.vm;
    this.manageFilesStore.getFiles();
    this.manageFilesStore.tableCreateResult$.subscribe(res => {
      if (res) {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Table created successfully' });
      } else {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Table creation failed' });
      }
    })
  }

  onDeleteFile(fileId: number): void {
    this.manageFilesStore.deleteFile({ fileId });
  }

  onCreateTable(fileId: number): void {
    this.manageFilesStore.createTable(fileId);
  }

}
