import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot } from '@angular/router';
import { CustomerService } from '../../services/customer.service';
import { CustomerDataService } from '../../services/customer/customer-data.service';
import { ShoppingCartDataService } from '../../services/shoppingcart/shoppingcart-data.service';

@Injectable({
    providedIn: 'root',
})
export class ShoppingGuard {
    constructor(
        private customerService: CustomerService,
        private customerDataService: CustomerDataService,
        private shoppingCartDataService: ShoppingCartDataService,
        private router: Router
    ) { }

    canActivate(state: RouterStateSnapshot): boolean {
        const shoppingCart = this.shoppingCartDataService.getShoppingCart();

        if (!shoppingCart?.items || shoppingCart.items.length === 0) {
            this.navigateToShoppingCart(state.url);
            return false;
        }

        if (!this.customerService.isLoggedIn()) {
            this.navigateToAuthentication(state.url);
            return false;
        }

        const customer = this.customerDataService.getCustomer();
        if (!customer?.shoppingCart?.items || customer.shoppingCart.items.length === 0) {
            this.navigateToShoppingCart(state.url);
            return false;
        }

        return true;
    }

    private navigateToShoppingCart(returnUrl: string): void {
        this.router.navigate(['/steps/my-shoppingcart'], { queryParams: { returnUrl } });
    }

    private navigateToAuthentication(returnUrl: string): void {
        this.router.navigate(['/steps/authentication'], { queryParams: { returnUrl } });
    }
}