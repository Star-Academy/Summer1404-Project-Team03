import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowWorkspaceComponent } from './workflow-workspace.component';

describe('WorkflowWorkspaceComponent', () => {
  let component: WorkflowWorkspaceComponent;
  let fixture: ComponentFixture<WorkflowWorkspaceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkflowWorkspaceComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(WorkflowWorkspaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
