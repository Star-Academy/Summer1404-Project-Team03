export interface FileManagmentState {
  fileItems: FileItem[];
  isLoadingFiles: boolean;
  error: string | null;
  deletingFileIds: number[];
}

export type FileItem = {
  id: number;
  originalFileName: string;
  stage: string;
  status: string;
  schemaId: number;
  fileSize: number;
  uploadedAt: string;
}

export interface UploadFileResponse {
  fileName: string;
  success: true;
  error: string;
  data: FileItem;
}