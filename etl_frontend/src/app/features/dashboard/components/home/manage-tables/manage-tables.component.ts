import {Component, computed, OnInit} from '@angular/core';
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

  ngOnInit() {
    this.tableStore.loadTables();
  }
}
