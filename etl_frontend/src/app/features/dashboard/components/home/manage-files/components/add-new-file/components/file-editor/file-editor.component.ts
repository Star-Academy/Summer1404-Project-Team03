import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TableModule } from 'primeng/table';
import { UploadFileStore } from '../../stores/upload-file/upload-file-store.service';
import { Select } from 'primeng/select';

export interface CsvColumnConfig {
  originalHeader: string;
  newHeader: string;
  selectedType: string;
}

@Component({
  selector: 'app-file-editor',
  imports: [TableModule, FormsModule, Select],
  templateUrl: './file-editor.component.html',
  styleUrl: './file-editor.component.scss'
})
export class FileEditorComponent implements OnInit {

  dbTypes = [
    { label: 'String', value: 'string' },
    { label: 'Number', value: 'number' },
    { label: 'Date', value: 'date' },
    { label: 'Boolean', value: 'boolean' }
  ];

  columnConfigurations: CsvColumnConfig[] = [];
  constructor(private activatRoute: ActivatedRoute, private readonly uploadFileStore: UploadFileStore) { }

  ngOnInit(): void {
    this.activatRoute.params.subscribe((params) => {
      const fileName = params['file-name'];
      if (fileName) {
        this.getFile(fileName);
      }
    });
  }

  getFile(fileName: string): void {
    const file = this.uploadFileStore.getFile(fileName);
    if(file) this.parseCsvForPreview(file);
  }

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
      }
    };

    reader.readAsText(file);
  }
}
