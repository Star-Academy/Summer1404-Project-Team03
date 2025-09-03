export interface UploadFileState {
  files: File[];
  isDragging: boolean;
  isUploading: boolean;
  error?: string | null;
  selectedFile?: File;
}