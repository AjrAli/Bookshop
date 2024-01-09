import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot } from '@angular/router';
import { CustomerService } from '../../services/customer.service';
import { IdleTimeoutService } from '../../services/idle-timeout.service';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard {

    constructor(private customerService: CustomerService, private router: Router, private idleTimeoutService: IdleTimeoutService) { }
    canActivate(state: RouterStateSnapshot): boolean {
        this.idleTimeoutService.onIdleTimeout().subscribe(() => {
            // Redirect to home page when idle timeout is reached
            if (!this.customerService.isLoggedIn()) {
                this.router.navigate(['/login']);
                this.idleTimeoutService.stopIdleTimer();
            }
        });
        if (!this.customerService.isLoggedIn()) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            return false;
        }
        return true;
    }
}