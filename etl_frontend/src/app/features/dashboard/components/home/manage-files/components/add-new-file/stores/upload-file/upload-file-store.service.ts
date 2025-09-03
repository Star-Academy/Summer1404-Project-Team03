import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { UploadFileState } from '../../models/file.model';

const initialState: UploadFileState = {
  files: [],
  isDragging: false,
  isUploading: false,
  error: null,
  selectedFile: undefined
};


@Injectable()
export class UploadFileStore extends ComponentStore<UploadFileState> {
  constructor() {
    super(initialState);
  }

  public readonly vm = this.selectSignal((s) => s);

  public readonly setDragging = this.updater<boolean>((state, isDragging) => ({
    ...state,
    isDragging,
  }));

  readonly addFile = this.updater<File>((state, file) => {
    if (!file.name.toLowerCase().endsWith('.csv')) {
      console.log('file is not valid');
      return { ...state, error: 'Only .csv files are allowed' };
    }

    console.log('file is valid and added');
    return {
      ...state,
      files: [...state.files, file],
      error: null
    };
  });

  public readonly removeFile = this.updater<string>((state, fileName) => ({
    ...state,
    files: state.files.filter((f) => f.name !== fileName),
  }));

  public readonly clearFiles = this.updater<void>((state) => ({
    ...state,
    files: [],
    error: null
  }));

  public readonly setError = this.updater<string | null>((state, error) => ({
    ...state,
    error,
  }));
}
