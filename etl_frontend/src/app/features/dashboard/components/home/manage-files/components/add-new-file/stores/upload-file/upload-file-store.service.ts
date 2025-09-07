import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { UploadFileState } from '../../models/file.model';
import { delay, exhaustMap, tap } from 'rxjs';
import { FilesManagementService } from '../../../../services/files-management/files-management.service';

const initialState: UploadFileState = {
  files: [],
  isDragging: false,
  isUploading: false,
  isSending: false,
  error: null,
  selectedFile: undefined
};


@Injectable()
export class UploadFileStore extends ComponentStore<UploadFileState> {
  constructor(private readonly filesManagementService: FilesManagementService) {
    super(initialState);
  }

  public readonly vm = this.selectSignal((s) => s);

  public readonly setDragging = this.updater<boolean>((state, isDragging) => ({
    ...state,
    isDragging,
  }));

  readonly addFile = this.updater<File>((state, file) => {
    if (!file.name.toLowerCase().endsWith('.csv')) {
      return { ...state, error: 'Only .csv files are allowed' };
    }

    return {
      ...state,
      files: [...state.files, file],
      error: null,
    };
  });

  readonly replaceFile = this.updater<File>((state, file) => {
    const files = state.files.map((f) => (f.name === file.name ? file : f));
    return { ...state, files };
  });

  public getFile(fileName: string): File | undefined {
    const state = this.get();
    return state.files.find((f) => f.name === fileName);
  }

  public readonly removeFile = this.updater<string>((state, fileName) => ({
    ...state,
    files: state.files.filter((f) => f.name !== fileName),
  }));

  public readonly clearFiles = this.updater<void>((state) => ({
    ...state,
    files: [],
    error: null,
  }));

  public readonly setError = this.updater<string | null>((state, error) => ({
    ...state,
    error,
  }));

  public readonly setSending = this.updater<boolean>((state, isSending) => ({
    ...state,
    isSending,
  }));

  public readonly uploadFiles = this.effect<void>(($trigger) =>
    $trigger.pipe(
      tap(() => this.setSending(true)),
      exhaustMap(() => {
        const files = this.getFilesFormData(this.get().files);
        return this.filesManagementService.uploadFiles(files).pipe(
          delay(3000),
          tap({
            next: (res) => {
              this.setSending(false);
              this.setError(null);
              this.get().files.forEach(file => this.removeFile(file.name))
              console.log(res);
            },
            error: (err) => {
              this.setSending(false);
              this.setError(err.message || 'Upload failed');
            },
          })
        );
      })
    )
  );

  public readonly uploadFileWithName = this.effect<{ fileName: string }>($trigger => {
    return $trigger.pipe(
      tap(() => this.setSending(true)),
      exhaustMap((data: { fileName: string }) => {
        const file = this.get().files.filter(file => file.name === data.fileName);

        const fileFormData = this.getFilesFormData(file);
        return this.filesManagementService.uploadFiles(fileFormData).pipe(
          tap({
            next: (res) => {
              this.setSending(false);
              this.setError(null);
              // this.getFile() //TODO remove the file
              console.log(res);
            },
            error: (err) => {
              this.setSending(false);
              this.setError(err.message || 'Upload failed');
            },
          })
        );
      })
    )
  })

  getFilesFormData(files: File[]): FormData {
    const formData: FormData = new FormData();
    files.forEach((file) => formData.append('Files', file));
    return formData;
  }
}
