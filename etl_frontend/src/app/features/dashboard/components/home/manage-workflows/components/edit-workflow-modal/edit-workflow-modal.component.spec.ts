import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWorkflowModalComponent } from './edit-workflow-modal.component';

describe('EditWorkflowModalComponent', () => {
  let component: EditWorkflowModalComponent;
  let fixture: ComponentFixture<EditWorkflowModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditWorkflowModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditWorkflowModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
