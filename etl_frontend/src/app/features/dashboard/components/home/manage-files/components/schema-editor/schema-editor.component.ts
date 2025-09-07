import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { Select } from 'primeng/select';

import { UploadFileStore } from '../add-new-file/stores/upload-file/upload-file-store.service';
import { CsvParserService } from '../../../../../../../shared/services/csv-parser/csv-parser.service';
import { ParsedCsvData } from '../../../../../../../shared/models/csv-parser.model';

export interface CsvColumnConfig {
  originalHeader: string;
  newHeader: string;
  selectedType: string;
}

@Component({
  selector: 'app-schema-editor',
  imports: [TableModule, FormsModule, Select, PanelModule],
  templateUrl: './schema-editor.component.html',
  styleUrl: './schema-editor.component.scss'
})
export class SchemaEditorComponent implements OnInit {
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
    // this.parseCsvForPreview();
  }

}