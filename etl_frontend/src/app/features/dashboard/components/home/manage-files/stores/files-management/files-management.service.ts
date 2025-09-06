import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { FileManagmentState } from '../../models/file.model';
import { finalize, switchMap, tap } from 'rxjs';
import { FilesManagementService } from '../../services/files-management/files-management.service';

const initialState: FileManagmentState = {
  isLoadingFiles: false,
  error: null,
  fileItems: []
}

@Injectable()
export class FilesManagementStore extends ComponentStore<FileManagmentState> {
  constructor(private readonly fileManagementService: FilesManagementService) {
    super(initialState);
  }

  public readonly vm = this.selectSignal(s => s);

  public readonly setLoadinFiles = this.updater((state, value: boolean) => ({
    ...state,
    isLoadingFiles: value,
  }));

  public readonly getFiles = this.effect<void>(($trigger) =>
    $trigger.pipe(
      tap(() => this.setLoadinFiles(true)),
      switchMap(() => this.fileManagementService.fetchFiles().pipe(
        finalize(() => this.setLoadinFiles(false)),
      )),
    )
  );
}