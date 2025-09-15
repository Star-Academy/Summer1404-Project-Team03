import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowsTabsManagementComponent } from './workflows-tabs-management.component';

describe('WorkflowsTabsManagementComponent', () => {
  let component: WorkflowsTabsManagementComponent;
  let fixture: ComponentFixture<WorkflowsTabsManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkflowsTabsManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowsTabsManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
