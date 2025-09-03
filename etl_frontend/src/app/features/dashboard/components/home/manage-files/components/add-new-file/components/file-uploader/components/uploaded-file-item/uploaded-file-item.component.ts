import { Component, input, output } from '@angular/core';
import { animate, style, transition, trigger } from "@angular/animations"
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-uploaded-file-item',
  imports: [RouterLink],
  templateUrl: './uploaded-file-item.component.html',
  styleUrl: './uploaded-file-item.component.scss',
  animations: [
    trigger('scaleAnimation', [
      transition(':enter', [
        style({ transform: 'scale(0.8)', opacity: 0 }),
        animate(
          '200ms ease-out',
          style({ transform: 'scale(1)', opacity: 1 })
        )
      ]),
    ])
  ]
})
export class UploadedFileItemComponent {
  public fileName = input<string>('');
  public removeFile = output<string>();
}
