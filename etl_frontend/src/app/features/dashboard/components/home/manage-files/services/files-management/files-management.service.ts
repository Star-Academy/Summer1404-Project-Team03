import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../../environments/environment';
import { Observable } from 'rxjs';
import { FileItem } from '../../models/file.model';
import { HttpClient } from '@angular/common/http';
import { Schema } from '../../components/schema-editor/models/schema.model';

@Injectable()
export class FilesManagementService {
  private readonly filesApi = environment.api.files;

  constructor(private readonly http: HttpClient) { }

  fetchFiles(): Observable<FileItem[]> {
    return this.http.get<FileItem[]>(this.filesApi.root)
  }

  fetchFileSchema(fileId: string): Observable<Schema> {
    return this.http.get<Schema>(this.filesApi.previewSchema(fileId));
  }
}
