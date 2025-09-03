import { NgClass } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { Button } from 'primeng/button';
import { UploadedFileItemComponent } from './components/uploaded-file-item/uploaded-file-item.component';
import { animate, style, transition, trigger } from "@angular/animations"
import { RouterOutlet } from '@angular/router';
import { UploadFileStore } from '../../stores/upload-file/upload-file-store.service';

@Component({
  selector: 'app-file-uploader',
  imports: [Button, NgClass, UploadedFileItemComponent, RouterOutlet],
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

  constructor(private uploadFileStore: UploadFileStore) {
    this.vm = this.uploadFileStore.vm;
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
      files.forEach((file: File) => this.uploadFileStore.addFile(file));
    }
  }

  onFileSelect(event: Event): void {
    const element = event.target as HTMLInputElement;
    const selectedFiles = element.files;

    if (selectedFiles) {
      Array.from(selectedFiles).forEach((file) =>
        this.uploadFileStore.addFile(file)
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
}
