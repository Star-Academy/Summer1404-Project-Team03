import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { FileManagmentState } from '../../models/file.model';

const initialState: FileManagmentState = {
  isLoadingFiles: false,
  error: null,
  fileItems: []
}

@Injectable()
export class FilesManagementStore extends ComponentStore<FileManagmentState> {

  constructor() {
    super(initialState);
  }
}