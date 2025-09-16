import { Component, computed } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ButtonGroupModule } from 'primeng/buttongroup';
import { ListboxModule } from 'primeng/listbox';
import { PopoverModule } from 'primeng/popover';
import { TableSelectorStore } from './stores/table-selector-store.service';
import { TableType } from '../../../../home/manage-tables/models/tables.model';
@Component({
  selector: 'app-workflow-table-selector',
  standalone: true,
  imports: [ButtonModule, ButtonGroupModule, PopoverModule, ListboxModule],
  templateUrl: './workflow-table-selector.component.html',
  styleUrls: ['./workflow-table-selector.component.scss']
})
export class WorkflowTableSelectorComponent {
  public readonly vm;

  tablesName = computed(() =>
    this.vm().tables.map((t: TableType) => ({
      name: t.tableName,
      value: t.schemaId
    }))
  );

  constructor(private readonly tableSelectorStore: TableSelectorStore) {
    this.vm = this.tableSelectorStore.vm;
  }

  onSelectTable(event: any): void {
    const selected = event.value;
    console.log('Selected table:', selected);
  }
}
