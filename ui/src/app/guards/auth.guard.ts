import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot, UrlSegment } from '@angular/router';
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
            const returnUrl = this.getCombinedReturnUrl(state.url);
            let queryParams: any = {};
            if (returnUrl)
                queryParams = { queryParams: { returnUrl: returnUrl } };
            this.router.navigate(['/login'], queryParams);
            return false;
        }
        return true;
    }
    private getCombinedReturnUrl(urls: any): string | null {
        const urlSegments: UrlSegment[] = urls;

        // Use the map function to extract the path from each UrlSegment
        const urlPath = urlSegments?.map((segment) => segment.path);

        // Join the array of path segments into a single string
        if (urlPath)
            return '/' + urlPath.join('/');
        else
            return null;
    }
}