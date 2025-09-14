import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SignInDirective } from './sign-in.directive';
import { Component } from '@angular/core';
import { Button } from 'primeng/button';

@Component({
  template: `<button pButton appSignIn></button>`
})
class TestHostComponent { }

describe('SignInDirective', () => {
  let fixture: ComponentFixture<TestHostComponent>;
  let directiveEl: HTMLElement;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TestHostComponent, SignInDirective],
      imports: [Button]
    })

    fixture = TestBed.createComponent(TestHostComponent);
    fixture.detectChanges();
    directiveEl = fixture.nativeElement.querySlector('button');
  })

  it('should create an instance', () => {
    expect(directiveEl).toBeTruthy();
  });
});
