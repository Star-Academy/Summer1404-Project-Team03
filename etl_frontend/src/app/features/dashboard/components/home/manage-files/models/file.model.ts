export interface FileManagmentState {
  fileItems: FileItem[];
  isLoadingFiles: boolean;
  error: string | null;
  deletingFileIds: number[];
}

export interface FileItem {
  id: number;
  originalFileName: string;
  stage: string;
  status: string;
  schemaId: number;
  fileSize: number;
  uploadedAt: string;
}