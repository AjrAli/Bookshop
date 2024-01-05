import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CustomerResponseDto } from '../../../dto/customer/customer-response-dto';
import { CustomerDataService, PaymentInformation } from '../../../../services/customer/customer-data.service';
import { combineLatest } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ListShopItemsComponent } from '../../../body/list-shop-items/list-shop-items.component';
import { ShoppingCartDataService } from '../../../../services/shoppingcart/shoppingcart-data.service';
import { ShoppingCartDetailsResponseDto } from '../../../dto/shoppingcart/shoppingcart-details-response-dto';
import { DividerModule } from 'primeng/divider';

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [CommonModule, ButtonModule, ListShopItemsComponent, DividerModule],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent implements OnInit {

  manage = 'show';
  rootUrl = environment.apiRootUrl;
  customer: CustomerResponseDto | null = null;
  shoppingcartDetails: ShoppingCartDetailsResponseDto | null = null;
  paymentInfo: PaymentInformation | null = null;
  constructor(private router: Router, private customerDataService: CustomerDataService, private shoppingCartDataService: ShoppingCartDataService) { }

  ngOnInit(): void {
    const customer$ = this.customerDataService.getCustomerObservable();
    const paymentInfo$ = this.customerDataService.getPaymentInformationObservable();
    const shoppingcartDetails = this.shoppingCartDataService.getShoppingCartDetails();
    // forjoin is used for join two observable into one
    const customerAndpaymentInfo$ = combineLatest({ customer: customer$, paymentInfo: paymentInfo$ });
    customerAndpaymentInfo$.subscribe({
      next: (r) => {
        if (r.customer && r.customer.shoppingCart && r.paymentInfo && shoppingcartDetails) {
          this.customer = r.customer;
          this.paymentInfo = r.paymentInfo;
          this.shoppingcartDetails = shoppingcartDetails;
        } else {
          this.router.navigate(['/steps/payment']);
        }
      }
    })

  }
  navigateToPrevious() {
    this.router.navigate(['/steps/payment'])
  }
  orderConfirmation() {
    console.log(this.customer);
  }
}
