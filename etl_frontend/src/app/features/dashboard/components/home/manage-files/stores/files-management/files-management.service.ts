import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { FileItem, FileManagmentState } from '../../models/file.model';
import { catchError, delay, exhaustMap, finalize, mergeMap, switchMap, tap, throwError } from 'rxjs';
import { FilesManagementService } from '../../services/files-management/files-management.service';
import { TableService } from '../../../manage-tables/services/table.service';

const initialState: FileManagmentState = {
  isLoadingFiles: false,
  error: null,
  fileItems: [],
  deletingFileIds: [],
  isCreatingTable: false
}

@Injectable()
export class FilesManagementStore extends ComponentStore<FileManagmentState> {
  constructor(
    private readonly fileManagementService: FilesManagementService,
    private readonly tableManagementService: TableService) {
    super(initialState);
  }

  public readonly vm = this.selectSignal(s => s);

  public readonly setLoadinFiles = this.updater<boolean>((state, value: boolean) => ({
    ...state,
    isLoadingFiles: value,
  }));

  public readonly setCreatingTable = this.updater<boolean>((state, value: boolean) => ({
    ...state,
    isCreatingTable: value,
  }));

  public readonly setFiles = this.updater<FileItem[]>((state, files: FileItem[]) => ({
    ...state,
    fileItems: files,
  }));

  public readonly setFileStage = this.updater<{ fileId: number, stage: string }>((state, data) => {
    const file = state.fileItems.filter(file => file.id === data.fileId)[0];
    file.stage = data.stage;
    return ({
      ...state,
      fileItems: [...state.fileItems, file],
    })
  });

  readonly addDeletingFileId = this.updater<number>((state, fileId) => ({
    ...state,
    deletingFileIds: [...state.deletingFileIds, fileId],
  })
  );

  readonly removeDeletingFileId = this.updater<number>((state, fileId) => ({
    ...state,
    deletingFileIds: state.deletingFileIds.filter((id) => id !== fileId),
  })
  );

  public readonly getFiles = this.effect<void>(($trigger) => {
    return $trigger.pipe(
      tap(() => this.setLoadinFiles(true)),
      switchMap(() => this.fileManagementService.fetchFiles().pipe(
        tap((files: FileItem[]) => this.setFiles(files)),
        finalize(() => { this.setLoadinFiles(false); }),
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
        tap(() => this.getFiles()),
        finalize(() => this.removeDeletingFileId(fileId))
      ))
    )
  });

  public readonly createTable = this.effect<number>(trigger$ => {
    return trigger$.pipe(
      tap(() => this.setCreatingTable(true)),
      exhaustMap((fileId: number) => this.tableManagementService.createTable(fileId).pipe(
        tap((res) => console.log(res)),
        tap((res) => this.setFileStage({ fileId: fileId, stage: 'Loaded' })),
        finalize(() => { this.setCreatingTable(false); }),
      ))
    )
  })
}
