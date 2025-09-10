import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { SchemaEditorStore } from './stores/schema-editor/schema-editor-store.service';
import { of } from 'rxjs';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-schema-editor',
  imports: [TableModule, FormsModule, PanelModule, SelectModule, InputTextModule, ButtonModule],
  providers: [SchemaEditorStore],
  templateUrl: './schema-editor.component.html',
  styleUrl: './schema-editor.component.scss'
})
export class SchemaEditorComponent implements OnInit {
  public readonly vm;
  constructor(
    private readonly activatRoute: ActivatedRoute,
    private readonly schemaEditorStore: SchemaEditorStore,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {
    this.vm = this.schemaEditorStore.vm;
  }

  dbTypes = [
    { label: 'string', value: 'string' },
    { label: 'number', value: 'number' },
    { label: 'boolean', value: 'boolean' },
    { label: 'float', value: 'float' },
  ]

  ngOnInit(): void {
    this.activatRoute.params.subscribe((params) => {
      const fileId = params['file-id'];
      if (fileId) {
        this.schemaEditorStore.getFileSchema(of({ fileId }));
        this.schemaEditorStore.getDbTypes();
      }
    });

    this.schemaEditorStore.isSaveSuccess$.subscribe((isSaveSuccess) => {
      if (isSaveSuccess) {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Schema updated successfully' });
        this.router.navigate(['/dashboard/files']);
      } else {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update schema' });
      }
    });
  }

  onSaveSchema(): void {
    this.schemaEditorStore.updateSchema();
  }
}
