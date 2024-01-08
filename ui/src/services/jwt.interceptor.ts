import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { ToastService } from './toast.service';
import { TokenService } from './token.service';
import { CustomerService } from './customer.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private router: Router,
        private toastService: ToastService,
        private tokenService: TokenService,
        private customerService: CustomerService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Get stored token from tokenService
        const token = this.tokenService.getToken();

        // Add in header Authorization Bearer + token for the current request if token exist
        if (token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }
        // Send updated request for next interceptor
        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 401) {
                    this.toastService.showSimpleError(error.message);
                    // Delete token invalide/expired and customer local data storage
                    this.customerService.resetFullyCustomer();
                    this.router.navigate(['/login']);
                }
                if(error.status === 0)
                    this.customerService.resetFullyCustomer();
                // Return error for other interceptors can handle this error too
                return throwError(() => error);
            })
        );
    }
}