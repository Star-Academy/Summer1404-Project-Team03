import { Injectable } from '@angular/core';
import { environment } from '../../../../../../../../environments/environment';
import { map, Observable } from 'rxjs';
import { FileItem, UploadFileResponse } from '../../models/file.model';
import { HttpClient } from '@angular/common/http';
import { Schema } from '../../components/schema-editor/models/schema.model';

@Injectable()
export class FilesManagementService {
  private readonly filesApi = environment.api.files;

  constructor(private readonly http: HttpClient) { }

  fetchFiles(): Observable<FileItem[]> {
    return this.http.get<{ items: FileItem[] }>(this.filesApi.root).pipe(
      map((res) => res.items)
    )
  }

  fetchFileSchema(fileId: string): Observable<Schema> {
    return this.http.get<Schema>(this.filesApi.previewSchema(fileId));
  }

  uploadFiles(files: FormData): Observable<UploadFileResponse> {
    return this.http.post<UploadFileResponse>(this.filesApi.upload, files);
  }

  deleteFile(fileId: number): Observable<void> {
    return this.http.delete<void>(this.filesApi.delete(fileId));
  }
}
