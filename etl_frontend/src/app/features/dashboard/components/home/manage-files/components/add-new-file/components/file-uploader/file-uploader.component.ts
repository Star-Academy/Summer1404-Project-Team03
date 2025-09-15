import { NgClass } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { Button } from 'primeng/button';
import { animate, style, transition, trigger } from "@angular/animations"
import { RouterLink } from '@angular/router';
import { UploadFileStore } from '../../stores/upload-file/upload-file-store.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProgressBarModule } from 'primeng/progressbar';
import { UploadedFileItemComponent } from './components/uploaded-file-item/uploaded-file-item.component';


@Component({
  selector: 'app-file-uploader',
  imports: [Button, NgClass, UploadedFileItemComponent, RouterLink, ProgressBarModule],
  templateUrl: './file-uploader.component.html',
  styleUrl: './file-uploader.component.scss',
  animations: [
    trigger('scaleAnimation', [
      transition(':leave', [
        animate(
          '150ms ease-in',
          style({ transform: 'scale(0.8)', opacity: 0 })
        ),
      ]),
    ]),
  ],
})
export class FileUploaderComponent {
  @ViewChild('fileInput') fileInput!: ElementRef;
  public readonly vm;

  constructor(
    private readonly uploadFileStore: UploadFileStore,
    private readonly confirmationService: ConfirmationService,
    private readonly messageService: MessageService) {
    this.vm = this.uploadFileStore.vm;

    this.uploadFileStore.uploadResult$.subscribe(res => {
      if (Array.isArray(res) && res.length > 0) {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'File(s) uploaded successfully' });
      } else if (res === 'error') {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Fail to upload File(s)' });
      }
    })
  }

  onDragOver(event: DragEvent): void {
    event.stopPropagation();
    event.preventDefault();
    this.uploadFileStore.setDragging(true);
  }

  onDragLeave(event: DragEvent): void {
    event.stopPropagation();
    event.preventDefault();
    this.uploadFileStore.setDragging(false);
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.uploadFileStore.setDragging(false);

    const droppedFiles = event.dataTransfer?.files;

    if (droppedFiles) {
      const files = Array.from(droppedFiles);
      files.forEach((file: File) => this.handleFile(file));
    }
  }

  onFileSelect(event: Event): void {
    const element = event.target as HTMLInputElement;
    const selectedFiles = element.files;

    if (selectedFiles) {
      Array.from(selectedFiles).forEach((file) =>
        this.handleFile(file)
      );
    }

    element.value = '';
  }

  removeFile(fileName: string): void {
    this.uploadFileStore.removeFile(fileName);
  }

  clearFiles(): void {
    this.uploadFileStore.clearFiles();
  }

  handleFile(file: File) {
    const currentFiles = this.uploadFileStore.vm().files;
    const exists = currentFiles.some(f => f.name === file.name);

    if (exists) {
      this.showReplacFileConfirmation(file);
    } else {
      this.uploadFileStore.addFile(file);
    }
  }

  showReplacFileConfirmation(file: File): void {
    this.confirmationService.confirm({
      message: `File "${file.name}" already exists. Replace it?`,
      header: 'Duplicate File',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Replace',
      rejectLabel: 'Ignore',
      accept: () => this.uploadFileStore.replaceFile(file),
      reject: () => { }
    });
  }

  onUploadAllFiles(): void {
    this.uploadFileStore.uploadFiles();
  }
}