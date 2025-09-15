import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowSidebarComponent } from './workflow-sidebar.component';

describe('WorkflowSidebarComponent', () => {
  let component: WorkflowSidebarComponent;
  let fixture: ComponentFixture<WorkflowSidebarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkflowSidebarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
