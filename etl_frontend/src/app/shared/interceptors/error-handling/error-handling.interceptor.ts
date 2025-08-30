import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { catchError, throwError } from 'rxjs';

export const errorHandlingInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const messageService = inject(MessageService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        router.navigate(['/login']);
        messageService.add({
          severity: 'warn',
          summary: 'Session Expired',
          detail: 'Your session has expired. Please log in again.',
          sticky: true,
        });
      } else {
        const errorMessage = error.error?.message || error.statusText || 'An unknown error occurred.';
        console.error(`HTTP Error: ${error.status}`, errorMessage);

        messageService.add({
          severity: 'error',
          summary: `Error ${error.status}`,
          detail: errorMessage,
          sticky: true,
        });
      }

      return throwError(() => error);
    })
  );
};
