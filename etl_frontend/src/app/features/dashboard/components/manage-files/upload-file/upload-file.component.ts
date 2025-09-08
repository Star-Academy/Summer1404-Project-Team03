import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Button, ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { FileUpload, FileUploadModule } from 'primeng/fileupload'
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';

export interface CsvColumnConfig {
  originalHeader: string;
  newHeader: string;
  selectedType: string;
}

@Component({
  selector: 'app-upload-file',
  imports: [
    CommonModule,
    FormsModule,
    FileUploadModule,
    ButtonModule,
    TableModule,
    InputTextModule,
    DropdownModule,
  ],
  templateUrl: './upload-file.component.html',
  styleUrl: './upload-file.component.scss'
})
export class UploadFileComponent {
  // Stores files selected by the user, but not yet uploaded
  selectedFiles: File[] = [];

  // Holds the configuration for the columns of the previewed file
  columnConfigurations: CsvColumnConfig[] = [];

  // Data types for the dropdown menu in the preview table
  dbTypes = [
    { label: 'Text', value: 'TEXT' },
    { label: 'Integer', value: 'INTEGER' },
    { label: 'Decimal', value: 'DECIMAL' },
    { label: 'Date', value: 'DATE' },
    { label: 'Boolean', value: 'BOOLEAN' }
  ];

  constructor(private messageService: MessageService) { }

  /**
   * Triggered when files are selected in the p-fileUpload component.
   * It populates the selected files list and parses the first valid CSV for preview.
   * @param event The file select event.
   */
  onFileSelect(event: any): void {
    this.selectedFiles = [...event.files];

    // Find the first CSV file to generate a preview
    const csvFile = this.selectedFiles.find(f => f.type === 'text/csv' || f.name.endsWith('.csv'));

    if (csvFile) {
      this.parseCsvForPreview(csvFile);
    } else {
      // Clear previous preview if no CSV is selected
      this.columnConfigurations = [];
    }
  }

  /**
   * Reads a CSV file and extracts its header row to build the configuration table.
   * @param file The CSV file to parse.
   */
  private parseCsvForPreview(file: File): void {
    const reader = new FileReader();

    reader.onload = (e) => {
      const text = reader.result as string;
      const lines = text.split(/\r\n|\n/); // Split into lines

      if (lines.length > 0) {
        const headers = lines[0].split(',').map(h => h.trim());
        this.columnConfigurations = headers.map(header => ({
          originalHeader: header,
          newHeader: header, // Initially, newHeader is same as original
          selectedType: 'TEXT' // Default to TEXT
        }));
        this.messageService.add({ severity: 'info', summary: 'Preview Ready', detail: 'Configure columns for the first selected file.' });
      }
    };

    reader.onerror = () => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Could not read the file.' });
    };

    reader.readAsText(file);
  }

  /**
   * This is the final "upload" action.
   * In a real app, you would send the files and the column configuration
   * to your backend service here.
   */
  processAndUploadFiles(): void {
    if (this.selectedFiles.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'No Files', detail: 'Please select files to upload.' });
      return;
    }

    // --- Mock Backend Call ---
    console.log('Uploading Files:', this.selectedFiles.map(f => f.name));
    console.log('With Column Configuration:', this.columnConfigurations);
    // -------------------------

    this.messageService.add({ severity: 'success', summary: 'Success', detail: `${this.selectedFiles.length} file(s) are being processed.` });

    // Reset the component state after "uploading"
    this.clearSelection();
  }

  /**
   * Clears the selected files and the configuration preview.
   */
  clearSelection(): void {
    this.selectedFiles = [];
    this.columnConfigurations = [];
  }

}
