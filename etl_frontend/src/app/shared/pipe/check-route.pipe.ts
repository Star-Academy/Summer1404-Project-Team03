import { Pipe, PipeTransform } from '@angular/core';
import { Router } from '@angular/router';

@Pipe({
  name: 'checkRoute'
})
export class CheckRoutePipe implements PipeTransform {
  constructor(private readonly router: Router) {
  }

  transform(expectedRoute: string): boolean {
    return this.router.url === expectedRoute;
  }

}
