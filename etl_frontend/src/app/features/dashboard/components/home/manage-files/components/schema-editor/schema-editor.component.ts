import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { SchemaEditorStore } from './stores/schema-editor/schema-editor-store.service';
import { of } from 'rxjs';

@Component({
  selector: 'app-schema-editor',
  imports: [TableModule, FormsModule, PanelModule],
  providers: [SchemaEditorStore],
  templateUrl: './schema-editor.component.html',
  styleUrl: './schema-editor.component.scss'
})
export class SchemaEditorComponent implements OnInit {
  public readonly vm;
  constructor(
    private readonly activatRoute: ActivatedRoute,
    private readonly schemaEditorStore: SchemaEditorStore
  ) {
    this.vm = this.schemaEditorStore.vm;
  }

  ngOnInit(): void {
    this.activatRoute.params.subscribe((params) => {
      const fileId = params['file-id'];
      if (fileId) {
        this.schemaEditorStore.getFileSchema(of({ fileId }));
      }
    });
  }
}
