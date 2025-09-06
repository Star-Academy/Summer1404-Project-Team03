import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { Select } from 'primeng/select';

import { UploadFileStore } from '../../stores/upload-file/upload-file-store.service';
import { CsvParserService } from '../../../../../../../../../shared/services/csv-parser/csv-parser.service';
import { ParsedCsvData } from '../../../../../../../../../shared/models/csv-parser.model';

export interface CsvColumnConfig {
  originalHeader: string;
  newHeader: string;
  selectedType: string;
}

@Component({
  selector: 'app-file-editor',
  imports: [TableModule, FormsModule, Select, PanelModule],
  templateUrl: './file-editor.component.html',
  styleUrl: './file-editor.component.scss'
})
export class FileEditorComponent implements OnInit {
  file: File | undefined;
  columnConfigurations: CsvColumnConfig[] = [];
  parsingErrors: string[] = [];
  dbTypes = [
    { label: 'String', value: 'string' },
    { label: 'Number', value: 'number' },
    { label: 'Date', value: 'date' },
    { label: 'Boolean', value: 'boolean' },
    { label: 'Text', value: 'text' }
  ];

  constructor(
    private activatRoute: ActivatedRoute,
    private readonly uploadFileStore: UploadFileStore,
    private csvParser: CsvParserService
  ) { }

  ngOnInit(): void {
    this.activatRoute.params.subscribe((params) => {
      const fileName = params['file-name'];
      if (fileName) {
        this.getFile(fileName);
      }
    });
  }

  getFile(fileName: string): void {
    this.file = this.uploadFileStore.getFile(fileName);
    this.parseCsvForPreview();
  }

  private parseCsvForPreview(): void {
    if (this.file) {
      this.parsingErrors = [];

      this.csvParser.parse(this.file).subscribe({
        next: (result: ParsedCsvData) => {
          // 2. Populate the array from the parsing result
          this.parsingErrors = result.errors;

          if (result.errors.length > 0) {
            console.warn('CSV parsing warnings:', result.errors);
          }

          this.columnConfigurations = result.headers.map(header => ({
            originalHeader: header,
            newHeader: header,
            selectedType: 'text'
          }));
        },
        error: (err) => {
          console.error('Failed to parse CSV file:', err);
          // Handle critical errors as well
          this.parsingErrors = [err.message || 'A critical error occurred while reading the file.'];
        }
      });
    }
  }
}