import { Component, Input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from '../../environments/environment';
import { TagModule } from 'primeng/tag';
import { ShopItemResponseDto } from '../../dto/shoppingcart/shopitem-response-dto';
import { ShoppingCartService } from '../../../services/shoppingcart.service';
@Component({
  selector: 'app-card',
  standalone: true,
  imports: [ButtonModule, TagModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  rootUrl = environment.apiRootUrl;
  @Input() book!: BookResponseDto;

  constructor(private shoppingCartService: ShoppingCartService) { }


  sendToShoppingCart(book: BookResponseDto) {
    this.shoppingCartService.addItem(book);
  }

}
