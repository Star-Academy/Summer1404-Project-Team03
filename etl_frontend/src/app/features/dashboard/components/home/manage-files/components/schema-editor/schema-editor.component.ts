import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { Select } from 'primeng/select';

import { SchemaEditorStore } from './stores/schema-editor/schema-editor-store.service';

export interface CsvColumnConfig {
  originalHeader: string;
  newHeader: string;
  selectedType: string;
}

@Component({
  selector: 'app-schema-editor',
  imports: [TableModule, FormsModule, Select, PanelModule],
  providers: [SchemaEditorStore],
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
    private readonly activatRoute: ActivatedRoute,
    private readonly schemaEditorStore: SchemaEditorStore
  ) {}

  ngOnInit(): void {
    this.activatRoute.params.subscribe((params) => {
      const fileName = params['file-name'];
      if (fileName) {
        this.getFile(fileName);
      }
    });
  }

  getFile(fileName: string): void {
    // this.file = this.uploadFileStore.getFile(fileName);
    // this.parseCsvForPreview();
  }

}