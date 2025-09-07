export interface UploadFileState {
  files: File[];
  isDragging: boolean;
  isUploading: boolean;
  isSending: boolean;
  error?: string | null;
  selectedFile?: File;
}