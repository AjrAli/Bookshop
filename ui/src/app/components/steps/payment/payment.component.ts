import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ListShopItemsComponent } from '../../../body/list-shop-items/list-shop-items.component';
import { environment } from '../../../environments/environment';
import { ShoppingCartDetailsResponseDto } from '../../../dto/shoppingcart/shoppingcart-details-response-dto';
import { ShoppingCartService } from '../../../../services/shoppingcart.service';
import { ShoppingCartDetailsResponse } from '../../../dto/handler-response/shoppingcart/shoppingcart-details-response';
import { PaymentFormComponent } from '../../../forms/payment-form/payment-form.component';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, ButtonModule, ListShopItemsComponent, PaymentFormComponent],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {
  @ViewChild('paymentForm') paymentForm!: PaymentFormComponent;
  manage = 'show';
  rootUrl = environment.apiRootUrl;
  shoppingcartDetails: ShoppingCartDetailsResponseDto | null = null;
  constructor(private shoppingCartService: ShoppingCartService,
    private router: Router) { }


  ngOnInit() {
    this.shoppingCartService.getShoppingCartDetailsToApi().subscribe({
      next: (response: ShoppingCartDetailsResponse | null) => {
        if (!response || !response.shoppingCartDetails) {
          this.shoppingcartDetails = null;
          this.router.navigate(['']);
          return;
        }
        this.shoppingcartDetails = response.shoppingCartDetails;
      }
    })
  }
  navigateToPrevious() {
    this.router.navigate(['/steps/my-shoppingcart'])
  }
  navigateToNext() {
    this.router.navigate(['/steps/confirmation'])
  }
}
