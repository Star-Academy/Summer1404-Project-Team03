import { Component, computed, input, OnInit, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TableModule } from 'primeng/table';
import { PanelModule } from 'primeng/panel';
import { SchemaEditorStore } from './stores/schema-editor/schema-editor-store.service';
import { of } from 'rxjs';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-schema-editor',
  imports: [TableModule, FormsModule, PanelModule, SelectModule, InputTextModule, ButtonModule, RouterLink, ProgressSpinnerModule],
  providers: [SchemaEditorStore],
  templateUrl: './schema-editor.component.html',
  styleUrl: './schema-editor.component.scss'
})
export class SchemaEditorComponent implements OnInit {
  public readonly vm;
  onSave = output<void>();
  fileId = input<number | undefined>(undefined);
  dbTypes = computed(() => {
    return this.vm().dbTypes.map(type => ({ label: type, value: type }))
  });
  constructor(
    private readonly activatRoute: ActivatedRoute,
    private readonly schemaEditorStore: SchemaEditorStore,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {
    this.vm = this.schemaEditorStore.vm;
  }

  ngOnInit(): void {
    if (this.fileId()) {
      this.getSchema(String(this.fileId()))
    } else {
      this.activatRoute.params.subscribe((params) => {
        const fileId = params['file-id'];
        if (fileId) {
          this.getSchema(fileId)
        }
      });
    }

    this.schemaEditorStore.isSaveSuccess$.subscribe((isSaveSuccess) => {
      if (isSaveSuccess === null) return;
      if (isSaveSuccess === true) {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Schema updated successfully' });
        this.router.navigate(['/dashboard/files']);
      } else if (isSaveSuccess === false) {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to update schema' });
      }
    });
  }

  getSchema(fileId: string): void {
    this.schemaEditorStore.getFileSchema(of({ fileId: fileId }));
    this.schemaEditorStore.getDbTypes();
  }

  onSaveSchema(): void {
    this.onSave.emit();
    this.schemaEditorStore.updateSchema();
  }
}
