import {Component, computed, OnInit, signal} from '@angular/core';
import {TableStoreService} from './stores/table-store.service';
import { TableService } from './services/table.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-manage-tables',
  standalone: false,
  templateUrl: './manage-tables.component.html',
  styleUrl: './manage-tables.component.scss'
})

export class ManageTablesComponent implements OnInit {
  public readonly tables = computed(() => this.tableStore.vm().tables);
  public readonly isLoading = computed(() => this.tableStore.vm().isLoading);

  public isRenameTabelModal = signal<boolean>(false);
  public schemaId = signal<string>('');
  public tableName = signal<string>('');

  constructor(
    private readonly tableStore: TableStoreService,
    private readonly tablesService: TableService,
    private readonly messageService: MessageService
  ) {}

  public onDeleteTable(schemaId: string) {
    this.tablesService.deleteTable(schemaId).subscribe({
      next: () => {
        this.tableStore.loadTables()
        this.messageService.add({
          severity: 'success',
          summary: 'Table deleted successfully',
        })
      }
    })
  }

  public onRenameTable(tableId: number, tableName: string) {
    console.log(tableId, tableName);
    this.schemaId.set(tableId.toString());
    this.tableName.set(tableName);
    this.changeTableRenameModalStatus();
  }

  public changeTableRenameModalStatus() {
    this.isRenameTabelModal.update(currentValue => !currentValue);
  }

  ngOnInit() {
    this.tableStore.loadTables();
  }
}


