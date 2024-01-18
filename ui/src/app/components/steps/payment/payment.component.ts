import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ListShopItemsComponent } from '../../../body/list-shop-items/list-shop-items.component';
import { environment } from '../../../environments/environment';
import { ShoppingCartDetailsResponseDto } from '../../../dto/shoppingcart/shoppingcart-details-response-dto';
import { ShoppingCartService } from '../../../../services/shoppingcart.service';
import { ShoppingCartDetailsResponse } from '../../../dto/handler-response/shoppingcart/shoppingcart-details-response';
import { PaymentFormComponent } from '../../../forms/payment-form/payment-form.component';
import { ToastService } from '../../../../services/toast.service';
import { CustomerDataService } from '../../../../services/customer/customer-data.service';
import { ShoppingCartDataService } from '../../../../services/shoppingcart/shoppingcart-data.service';
import { ShoppingCartApiService } from '../../../../services/shoppingcart/shoppingcart-api.service';
import { Subscription } from 'rxjs';
import { ErrorResponse } from '../../../dto/response/error/error-response';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, ButtonModule, ListShopItemsComponent, PaymentFormComponent],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit, OnDestroy {
  @ViewChild('paymentForm') paymentForm!: PaymentFormComponent;
  manage = 'show';
  rootUrl = environment.apiRootUrl;
  shoppingcartDetails: ShoppingCartDetailsResponseDto | null = null;
  private shoppingCartSubscription: Subscription | undefined;
  constructor(private shoppingCartApiService: ShoppingCartApiService,
    private shoppingCartDataService: ShoppingCartDataService,
    private customerDataService: CustomerDataService,
    private router: Router, private toastService: ToastService) { }

  ngOnDestroy(): void {
    this.shoppingCartSubscription?.unsubscribe();
  }
  ngOnInit() {
    this.shoppingCartSubscription = this.shoppingCartApiService.getShoppingCartDetails().subscribe({
      next: (response: ShoppingCartDetailsResponse | null) => {
        if (!response || !response.shoppingCartDetails || response.shoppingCartDetails?.items?.length === 0) {
          this.shoppingcartDetails = null;
          this.router.navigate(['']);
          return;
        }
        this.shoppingcartDetails = response.shoppingCartDetails;
        this.shoppingCartDataService.setShoppingCartDetails(response.shoppingCartDetails);
      },
      error: (e: ErrorResponse) => {
        this.toastService.showError(e);
        this.router.navigate(['']);
      }
    });
  }
  navigateToPrevious() {
    this.router.navigate(['/steps/my-shoppingcart'])
  }
  navigateToNext() {
    const paymentForm = this.paymentForm.getFormData();
    if (this.paymentForm.paymentForm.valid && paymentForm) {
      this.customerDataService.setPaymentInformation(paymentForm)
      this.router.navigate(['/steps/confirmation'])
    } else {
      this.toastService.showSimpleError('Valid payment form is required');
    }
  }
}
