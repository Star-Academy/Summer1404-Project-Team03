import { Component, input } from '@angular/core';
import { WorkflowInfo } from '../../../../models/workflow.model';

@Component({
  selector: 'app-workflow-tab',
  standalone: false,
  templateUrl: './workflow-tab.component.html',
  styleUrl: './workflow-tab.component.scss'
})
export class WorkflowTabComponent {
  workflowInfo = input.required<WorkflowInfo>();
}
