import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanel } from 'primeng/overlaypanel';
import { Subscription } from 'rxjs';
import { ShoppingCartDataService } from '../../../../services/shoppingcart/shoppingcart-data.service';
import { ListShopItemsComponent } from '../../../body/list-shop-items/list-shop-items.component';
import { ShoppingCartResponseDto } from '../../../dto/shoppingcart/shoppingcart-response-dto';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, InputGroupModule, ButtonModule, InputTextModule, ListShopItemsComponent],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {
  manage = true;
  rootUrl = environment.apiRootUrl;
  private shoppingCartSubscription: Subscription | undefined;
  shoppingcart: ShoppingCartResponseDto | null = new ShoppingCartResponseDto();
  @ViewChild('op') op!: OverlayPanel;
  constructor(private shoppingCartDataService: ShoppingCartDataService,
    private router: Router) { }
  ngOnDestroy(): void {
    if (this.shoppingCartSubscription)
      this.shoppingCartSubscription.unsubscribe();
  }

  ngOnInit() {
    this.shoppingCartSubscription = this.shoppingCartDataService.getShoppingCartObservable().subscribe({
      next: (response: ShoppingCartResponseDto | null) => {
        if (!response || response.items.length === 0) {
          this.shoppingcart = null;
          this.router.navigate(['']);
          return;
        }
        this.shoppingcart = response;
      }
    })
  }
  getTotalItems() {
    return this.shoppingcart?.getTotalItems() ?? 0;
  }
  navigateToPrevious() {
    this.router.navigate(['/steps/my-shoppingcart'])
  }
  navigateToNext() {
    this.router.navigate(['/steps/confirmation'])
  }
}
