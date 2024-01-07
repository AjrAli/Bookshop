import { Component, Input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from '../../environments/environment';
import { TagModule } from 'primeng/tag';
import { ShoppingCartService } from '../../../services/shoppingcart.service';
import { Router } from '@angular/router';
import { ImageModule } from 'primeng/image';
@Component({
  selector: 'app-card',
  standalone: true,
  imports: [ButtonModule, TagModule, ImageModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  rootUrl = environment.apiRootUrl;
  @Input() book!: BookResponseDto;

  constructor(private shoppingCartService: ShoppingCartService,
              private router: Router) { }


  sendToShoppingCart(book: BookResponseDto) {
    this.shoppingCartService.addItem(book);
  }

  navigateToDetails(book: BookResponseDto){
    this.router.navigate([`/book/${book.id}`]);
  }

}
