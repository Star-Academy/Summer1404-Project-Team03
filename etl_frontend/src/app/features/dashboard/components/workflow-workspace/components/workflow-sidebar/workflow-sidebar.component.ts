import { Component } from '@angular/core';
import { PluginToolboxComponent } from './plugin-toolbox/plugin-toolbox.component';
import { WorkflowTableSelectorComponent } from './workflow-table-selector/workflow-table-selector.component';

@Component({
  selector: 'app-workflow-sidebar',
  imports: [PluginToolboxComponent, WorkflowTableSelectorComponent],
  templateUrl: './workflow-sidebar.component.html',
  styleUrl: './workflow-sidebar.component.scss'
})
export class WorkflowSidebarComponent {

}
