import { Component, ViewChild, OnInit } from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup'
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';
import { CustomerService } from '../../../services/customer.service';
import { ShoppingCartService } from '../../../services/shoppingcart.service';
import { ShoppingCartResponseDto } from '../../dto/shoppingcart/shoppingcart-response-dto';
import { ShopItemResponseDto } from '../../dto/shoppingcart/shopitem-response-dto';
import { BookService } from '../../../services/book.service';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { ErrorResponse } from '../../dto/response/error/error-response';
import { ListShopItemsComponent } from '../../body/list-shop-items/list-shop-items.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-actions',
  standalone: true,
  imports: [InputGroupModule, ButtonModule, InputTextModule, OverlayPanelModule, ScrollPanelModule, CommonModule, ListShopItemsComponent],
  templateUrl: './top-actions.component.html',
  styleUrl: './top-actions.component.css'
})
export class TopActionsComponent implements OnInit {
  rootUrl = environment.apiRootUrl;
  shoppingcart: ShoppingCartResponseDto | null = new ShoppingCartResponseDto();
  totalItems: number = 0;
  @ViewChild('op') op!: OverlayPanel;
  constructor(private bookService: BookService,
    private customerService: CustomerService,
    private shoppingCartService: ShoppingCartService,
    private router: Router) { }

  ngOnInit() {
    this.shoppingCartService.getShoppingCartObservable().subscribe({
      next: (response: ShoppingCartResponseDto | null) => {
        if (!response || response.items.length === 0) {
          this.shoppingcart = null;
          return;
        }
        this.shoppingcart = response;
        this.totalItems = response.getTotalItems();
      }
    })
  }
  goToShoppingCart(event: any) {
    if (this.op) {
      this.op.toggle(event);
      this.router.navigate(['/login']);
    }
  }
}

