import {Component, computed, effect, OnInit, signal} from '@angular/core';
import {TableRowStoreService} from './stores/table-row-store.service';
import {ActivatedRoute, Router} from '@angular/router';
import {CommonModule} from '@angular/common';
import {ButtonModule} from 'primeng/button';
import {TableModule} from 'primeng/table';
import {CardModule} from 'primeng/card';
import {TableRowService} from './services/table-row.service';

@Component({
  selector: 'app-table-rows',
  imports: [
    CommonModule,
    ButtonModule,
    TableModule,
    CardModule,
  ],
  templateUrl: './table-rows.component.html',
  styleUrl: './table-rows.component.scss',
  providers: [TableRowStoreService, TableRowService],
})
export class TableRowsComponent implements OnInit {
  private tableId = signal<string>('');
  public readonly rows = computed(() => this.rowStore.vm().rows);
  public readonly transformedRows = signal<{ name: string, value: any }[]>([]);
  public readonly isLoading = computed(() => this.rowStore.vm().isLoading);

  constructor(
    private readonly rowStore: TableRowStoreService,
    private readonly activatRoute: ActivatedRoute,
    private readonly router: Router
  ) {
    effect(() => {
      const formattedData = this.rows()
        .flatMap(row => Object.entries(row).map(([key, value]) => ({
          name: key,
          value: value,
        })));

      this.transformedRows.set(formattedData);
    });
  }

  ngOnInit() {
    this.activatRoute.params.subscribe((params) => {
      const tableId = params['table-id'];
      if (tableId) {
        this.tableId.set(tableId);
        this.rowStore.loadRows(+tableId)
      } else this.router.navigateByUrl('/dashboard/tables/list');
    });
  }
}
