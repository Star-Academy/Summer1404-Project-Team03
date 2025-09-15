import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowTableSelectorComponent } from './workflow-table-selector.component';

describe('WorkflowTableSelectorComponent', () => {
  let component: WorkflowTableSelectorComponent;
  let fixture: ComponentFixture<WorkflowTableSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkflowTableSelectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowTableSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
