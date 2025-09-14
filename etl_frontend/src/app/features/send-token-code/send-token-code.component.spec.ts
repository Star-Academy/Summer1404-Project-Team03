import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendTokenCodeComponent } from './send-token-code.component';

describe('SendTokenCodeComponent', () => {
  let component: SendTokenCodeComponent;
  let fixture: ComponentFixture<SendTokenCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SendTokenCodeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SendTokenCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('')
});
