import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-table-schema',
  imports: [],
  templateUrl: './table-schema.component.html',
  styleUrl: './table-schema.component.scss'
})
export class TableSchemaComponent implements OnInit{
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
