import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { FileItem, FileManagmentState } from '../../models/file.model';
import { delay, finalize, mergeMap, switchMap, tap } from 'rxjs';
import { FilesManagementService } from '../../services/files-management/files-management.service';

const initialState: FileManagmentState = {
  isLoadingFiles: false,
  error: null,
  fileItems: [],
  deletingFileIds: []
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

  public readonly setFiles = this.updater((state, files: FileItem[]) => ({
    ...state,
    fileItems: files,
  }));

  readonly addDeletingFileId = this.updater<FileManagmentState, number>(
    (state, fileId) => ({
      ...state,
      deletingFileIds: [...state.deletingFileIds, fileId],
    })
  );

  readonly removeDeletingFileId = this.updater<FileManagmentState, number>(
    (state, fileId) => ({
      ...state,
      deletingFileIds: state.deletingFileIds.filter((id) => id !== fileId),
    })
  );

  public readonly getFiles = this.effect<void>(($trigger) => {
    return $trigger.pipe(
      tap(() => this.setLoadinFiles(true)),
      switchMap(() => this.fileManagementService.fetchFiles().pipe(
        tap((files: FileItem[]) => this.setFiles(files)),
        finalize(() => { this.setLoadinFiles(false); this.getFiles() }),
      )),
    )
  }
  );

  public readonly deleteFile = this.effect<{ fileId: number }>(($trigger) => {
    let fileId: number;
    return $trigger.pipe(
      tap((data) => { this.addDeletingFileId(data.fileId); fileId = data.fileId; }),
      mergeMap((data: { fileId: number }) => this.fileManagementService.deleteFile(data.fileId).pipe(
        delay(3000),
        finalize(() => this.removeDeletingFileId(fileId))
      ))
    )
  })
}
