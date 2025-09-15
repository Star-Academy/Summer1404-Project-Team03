import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowSelectorComponent } from './workflow-selector.component';

describe('WorkflowSelectorComponent', () => {
  let component: WorkflowSelectorComponent;
  let fixture: ComponentFixture<WorkflowSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkflowSelectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
