import {Component, computed, OnInit, signal} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {TableColumnStoreService} from './stores/table-column-store.service';
import {TableColumnService} from './services/table-column.service';
import {MessageService} from 'primeng/api';
import {RenameColumnComponent} from './components/rename-column/rename-column.component';
import {TooltipModule} from 'primeng/tooltip';
import {TableModule} from 'primeng/table';
import {CardModule} from 'primeng/card';
import {CommonModule} from '@angular/common';
import {ButtonModule} from 'primeng/button';
import {MessageModule} from 'primeng/message';

@Component({
  selector: 'app-table-schema',
  imports: [
    CommonModule,
    RenameColumnComponent,
    ButtonModule,
    TooltipModule,
    TableModule,
    CardModule,
    MessageModule
  ],
  templateUrl: './table-column.component.html',
  styleUrl: './table-column.component.scss',
  providers: [TableColumnStoreService, TableColumnService]
})

export class TableColumnComponent implements OnInit {
  private tableId = signal<string>('');
  public readonly columns = computed(() => this.columnStore.vm().columns);
  public readonly isLoading = computed(() => this.columnStore.vm().isLoading);
  public readonly columnId = signal<number>(0);
  public readonly columnName = signal<string>('');
  public isRenameColumnModal = signal<boolean>(false);

  constructor(
    private readonly columnStore: TableColumnStoreService,
    private readonly columnService: TableColumnService,
    private readonly messageService: MessageService,
    private readonly activatRoute: ActivatedRoute,
    private readonly router: Router
  ) {}

  public setColumnId(name: string, id: number) {
    this.columnName.set(name);
    this.columnId.set(id);
    this.changeRenameColumnModalStatus();
  }

  public changeRenameColumnModalStatus(): void {
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
        this.columnStore.loadColumn(+tableId)
      } else this.router.navigateByUrl('/dashboard/tables/list');
    });
  }
}
