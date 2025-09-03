import { NgClass } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { Button } from 'primeng/button';

@Component({
  selector: 'app-file-uploader',
  imports: [Button, NgClass],
  templateUrl: './file-uploader.component.html',
  styleUrl: './file-uploader.component.scss'
})
export class FileUploaderComponent {
  @ViewChild('fileInput') fileInput!: ElementRef;

  isDragging = false;
  files: File[] = [];

  onDragOver(event: DragEvent): void {
    // This is crucial to allow a drop.
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    const droppedFiles = event.dataTransfer?.files;
    console.log(droppedFiles);
    if (droppedFiles) {
      this.handleFiles(Array.from(droppedFiles));
    }
  }

  // --- File Input and Selection Handlers ---

  // Triggers the hidden file input
  openFilePicker(): void {
    this.fileInput.nativeElement.click();
  }

  // Handles files selected via the file dialog
  onFileSelect(event: Event): void {
    const element = event.target as HTMLInputElement;
    const selectedFiles = element.files;
    if (selectedFiles) {
      this.handleFiles(Array.from(selectedFiles));
    }
    // Clear the input value to allow selecting the same file again
    element.value = '';
  }

  // --- File Management ---

  private handleFiles(newFiles: File[]): void {
    // You can add validation here (file type, size, etc.)
    // For example, to prevent duplicates:
    // const uniqueFiles = newFiles.filter(newFile => 
    //   !this.files.some(existingFile => existingFile.name === newFile.name)
    // );
    this.files.push(...newFiles);
  }

  removeFile(fileToRemove: File): void {
    this.files = this.files.filter(file => file !== fileToRemove);
  }

  clearFiles(): void {
    this.files = [];
  }
}
