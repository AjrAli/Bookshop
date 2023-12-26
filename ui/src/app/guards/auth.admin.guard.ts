import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { CustomerService } from '../../services/customer.service';

@Injectable({
    providedIn: 'root',
})
export class AuthAdminGuard {

    constructor(private customerService: CustomerService, private router: Router) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (!this.customerService.isAdmin()) {
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
            return false;
        }
        return true;
    }
}