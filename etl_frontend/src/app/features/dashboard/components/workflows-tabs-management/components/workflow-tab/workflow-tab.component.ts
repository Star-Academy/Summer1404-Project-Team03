import { Component, input, output } from '@angular/core';
import { WorkflowInfo } from '../../../../models/workflow.model';

@Component({
  selector: 'app-workflow-tab',
  standalone: false,
  templateUrl: './workflow-tab.component.html',
  styleUrl: './workflow-tab.component.scss',
})
export class WorkflowTabComponent {
  public workflowInfo = input.required<WorkflowInfo>();
  public isSelected = input.required<boolean>();
  public opneWorkflow = output<string>();
  public closeWorkflow = output<string>();

  public onOpenWorkflow(): void {
    this.opneWorkflow.emit(this.workflowInfo().id);
  }

  public onCloseWorkflow(): void {
    this.closeWorkflow.emit(this.workflowInfo().id);
  }
}
