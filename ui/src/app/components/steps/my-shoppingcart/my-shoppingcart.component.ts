import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanel } from 'primeng/overlaypanel';
import { Subscription } from 'rxjs';
import { BookService } from '../../../../services/book.service';
import { CustomerService } from '../../../../services/customer.service';
import { ShoppingCartService } from '../../../../services/shoppingcart.service';
import { ListShopItemsComponent } from '../../../body/list-shop-items/list-shop-items.component';
import { ShoppingCartResponseDto } from '../../../dto/shoppingcart/shoppingcart-response-dto';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-my-shoppingcart',
  standalone: true,
  imports: [CommonModule, InputGroupModule, ButtonModule, InputTextModule, ListShopItemsComponent],
  templateUrl: './my-shoppingcart.component.html',
  styleUrl: './my-shoppingcart.component.css'
})
export class MyShoppingCartComponent {
  manage = true;
  rootUrl = environment.apiRootUrl;
  private shoppingCartSubscription: Subscription | undefined;
  shoppingcart: ShoppingCartResponseDto | null = new ShoppingCartResponseDto();
  @ViewChild('op') op!: OverlayPanel;
  constructor(private bookService: BookService,
    private customerService: CustomerService,
    private shoppingCartService: ShoppingCartService,
    private router: Router) { }
  ngOnDestroy(): void {
    if (this.shoppingCartSubscription)
      this.shoppingCartSubscription.unsubscribe();
  }

  ngOnInit() {
    this.shoppingCartSubscription = this.shoppingCartService.getShoppingCartObservable().subscribe({
      next: (response: ShoppingCartResponseDto | null) => {
        if (!response || response.items.length === 0) {
          this.shoppingcart = null;
          return;
        }
        this.shoppingcart = response;
      }
    })
  }
  getTotalItems() {
    return this.shoppingcart?.getTotalItems() ?? 0;
  }
  goToShoppingCart(event: any) {
    if (this.op) {
      this.op.toggle(event);
      this.router.navigate(['/my-shoppingcart']);
    }
  }
}
