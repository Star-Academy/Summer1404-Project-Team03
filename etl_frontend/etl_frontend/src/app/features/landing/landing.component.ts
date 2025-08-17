import { Component } from '@angular/core';
import {Toolbar} from 'primeng/toolbar';
import {Button} from 'primeng/button';
import {NgOptimizedImage} from '@angular/common';

@Component({
  selector: 'app-landing',
  imports: [
    Toolbar,
    Button,
    NgOptimizedImage
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {}
