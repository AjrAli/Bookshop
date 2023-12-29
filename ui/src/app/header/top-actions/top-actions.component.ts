import { Component, OnInit } from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup'
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { OverlayPanelModule } from 'primeng/overlaypanel';
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

@Component({
  selector: 'app-top-actions',
  standalone: true,
  imports: [InputGroupModule, ButtonModule, InputTextModule, OverlayPanelModule, ScrollPanelModule, CommonModule, ListShopItemsComponent],
  templateUrl: './top-actions.component.html',
  styleUrl: './top-actions.component.css'
})
export class TopActionsComponent implements OnInit {
  rootUrl = environment.apiRootUrl;
  shoppingcart: ShoppingCartResponseDto = new ShoppingCartResponseDto();
  constructor(private bookService: BookService,
    private customerService: CustomerService,
    private shoppingCartService: ShoppingCartService) { }

  ngOnInit() {
    this.shoppingcart.total = 2500;
    this.bookService.getAll().subscribe({
      next: (response: any) => {
        const books: BookResponseDto[] = response.listDto
        if (books?.length > 0) {
          for (let book of books) {
            let shopitem = new ShopItemResponseDto(0, 3, book.id, book.price * 3, book.title, book.imageUrl, book.authorName, book.categoryTitle);
            this.shoppingcart.items.push(shopitem);
          }
        }
      }
    })
  }
}
