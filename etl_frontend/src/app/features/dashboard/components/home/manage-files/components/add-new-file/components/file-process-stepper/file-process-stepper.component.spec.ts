import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileProcessStepperComponent } from './file-process-stepper.component';

describe('FileProcessStepperComponent', () => {
  let component: FileProcessStepperComponent;
  let fixture: ComponentFixture<FileProcessStepperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FileProcessStepperComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FileProcessStepperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
