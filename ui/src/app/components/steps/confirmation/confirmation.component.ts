import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CustomerResponseDto } from '../../../dto/customer/customer-response-dto';
import { CustomerDataService, PaymentInformation } from '../../../../services/customer/customer-data.service';
import { environment } from '../../../environments/environment';
import { ListShopItemsComponent } from '../../../body/list-shop-items/list-shop-items.component';
import { ShoppingCartDataService } from '../../../../services/shoppingcart/shoppingcart-data.service';
import { ShoppingCartDetailsResponseDto } from '../../../dto/shoppingcart/shoppingcart-details-response-dto';
import { DividerModule } from 'primeng/divider';
import { OrderResponseDto } from '../../../dto/order/order-response-dto';
import { OrderDto } from '../../../dto/order/order-dto';
import { OrderService } from '../../../../services/order.service';
import { CustomerService } from '../../../../services/customer.service';
import { OrderDetailsComponent } from '../../order-details/order-details.component';
import { Subscription } from 'rxjs';
import { ShoppingCartService } from '../../../../services/shoppingcart.service';

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [CommonModule, ButtonModule, ListShopItemsComponent, DividerModule, OrderDetailsComponent],
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css'
})
export class ConfirmationComponent implements OnInit, OnDestroy {

  manage = 'show';
  rootUrl = environment.apiRootUrl;
  customer: CustomerResponseDto | null = null;
  shoppingcartDetails: ShoppingCartDetailsResponseDto | null = null;
  paymentInfo: PaymentInformation | null = null;
  order: OrderResponseDto | null = null;
  private orderSubscription: Subscription | undefined;
  private shoppingCartSubscription: Subscription | undefined;
  constructor(private router: Router,
    private customerDataService: CustomerDataService,
    private customerService: CustomerService,
    private shoppingCartDataService: ShoppingCartDataService,
    private shoppingCartService: ShoppingCartService,
    private orderService: OrderService) { }

  ngOnDestroy(): void {
    this.orderSubscription?.unsubscribe();
    this.shoppingCartSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.paymentInfo = this.customerDataService.getPaymentInformation();
    this.shoppingcartDetails = this.shoppingCartDataService.getShoppingCartDetails();
    this.customer = this.customerDataService.getCustomer();
    if (!this.paymentInfo && !this.shoppingcartDetails) {
      this.router.navigate(['/steps/payment']);
    }
  }

  navigateToPrevious() {
    this.router.navigate(['/steps/payment'])
  }
  returnToIndex() {
    this.router.navigate(['']);
  }
  orderConfirmation() {
    if (this.customer && this.customer.shoppingCart && this.paymentInfo && this.shoppingcartDetails) {
      const orderDto = new OrderDto(this.paymentInfo);
      if (orderDto.methodOfPayment) {
        this.orderSubscription = this.orderService.createOrderFromApi(orderDto).subscribe({
          next: (r) => {
            if (r) {
              this.order = r;
              this.resetAllDataRelatedToPreviousOrder();
            }
          },
          error: () => {
            //As an error occured we reset all shoppingcart local data then we set the available shoppingcart updated from API as default
            //Then we redirect to my-shoppingcart if it still items on the shoppingcart received if not it's back to index page
            this.resetAllDataRelatedToPreviousOrder();
            this.redirectOnSetAvailableShoppingCartFromApi();
          }
        })
      }
    }
  }
  redirectOnSetAvailableShoppingCartFromApi() {
    this.shoppingCartSubscription = this.shoppingCartService.setAvailableShoppingCartFromApi().subscribe({
      next: (response) => {
        if (response) {
          this.router.navigate(['/steps/my-shoppingcart']);
        } else {
          this.router.navigate(['']);
        }
      },
      error: () => {
        this.router.navigate(['']);
      }
    });
  }
  resetAllDataRelatedToPreviousOrder() {
    this.customerService.resetFullyLocalShoppingCart();
    this.customerDataService.resetPaymentInformation();
    this.paymentInfo = null;
    this.shoppingcartDetails = null;
  }

}
