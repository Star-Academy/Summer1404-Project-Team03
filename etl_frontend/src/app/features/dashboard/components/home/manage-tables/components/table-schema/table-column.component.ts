import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TableStoreService } from './stores/table-column-store.service';
import { TableColumnService } from './services/table-column.service';

@Component({
  selector: 'app-table-schema',
  imports: [],
  templateUrl: './table-schema.component.html',
  styleUrl: './table-schema.component.scss',
  providers: [TableStoreService, TableColumnService]
})
export class TableColumnComponent implements OnInit{
  constructor(private readonly activatRoute: ActivatedRoute) {}

  ngOnInit() {
    this.activatRoute.params.subscribe((params) => {
      const tableId = params['table-id'];
      if (tableId) {
        //TODO complete this
      }
    });
  }
}
