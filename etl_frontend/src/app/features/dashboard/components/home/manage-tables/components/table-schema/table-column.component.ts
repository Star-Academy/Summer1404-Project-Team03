import { Component, computed, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TableColumnStoreService } from './stores/table-column-store.service';
import { TableColumnService } from './services/table-column.service';
import { MessageService } from 'primeng/api';
import { RenameColumnComponent } from './components/rename-column/rename-column.component';

@Component({
  selector: 'app-table-schema',
  imports: [RenameColumnComponent],
  templateUrl: './table-schema.component.html',
  styleUrl: './table-schema.component.scss',
  providers: [TableColumnStoreService, TableColumnService]
})
export class TableColumnComponent implements OnInit{
  private tableId!: WritableSignal<string>;
  public readonly columns = computed(() => this.columnStore.vm().columns);
  public readonly isLoading = computed(() => this.columnStore.vm().isLoading);
  public readonly columnId!: WritableSignal<string>;
  public readonly columnName!: WritableSignal<string>;
  public isRenameColumnModal = signal<boolean>(false);

  constructor(
    private readonly columnStore: TableColumnStoreService,
    private readonly columnService: TableColumnService,
    private readonly messageService: MessageService,
    private readonly activatRoute: ActivatedRoute,
    private readonly router: Router
  ) {}

  public changeRenameColumnModalStatus(name: string, id: string): void {
    this.columnName.set(name);
    this.columnId.set(id);
    this.isRenameColumnModal.update(currentValue => !currentValue);
  }

  public onDeleteColumn(columnId: number) {
    this.columnService.deleteTableColumn(+this.tableId(), [columnId]).subscribe({
      next: () => {
        this.columnStore.loadColumn(+this.tableId());
        this.messageService.add({
          severity: 'success',
          summary: 'Successfully deleted column',
        })
      }
    })
  }

  ngOnInit() {
    this.activatRoute.params.subscribe((params) => {
      const tableId = params['table-id'];
      if (tableId) {
        this.tableId.set(tableId);
        this.columnStore.loadColumn(tableId)
      } else this.router.navigateByUrl('/dashboard/tables/list');
    });
  }
}
