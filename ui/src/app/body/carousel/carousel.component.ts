import { Component, Input } from '@angular/core';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from "../../../app/environments/environment";
import { ShoppingCartService } from '../../../services/shoppingcart.service';
import { Router } from '@angular/router';
import { ImageModule } from 'primeng/image';
@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [CarouselModule, ButtonModule, TagModule, ImageModule],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css'
})
export class CarouselComponent {
  rootUrl = environment.apiRootUrl;
  @Input() verticalViewPortHeight!: string;
  @Input() orientation!: any;
  @Input() numVisible!: number;
  @Input() books: BookResponseDto[] | undefined;
  @Input() responsiveOptions: any[] | undefined;

  constructor(private shoppingCartService: ShoppingCartService, private router: Router) { }

  sendToShoppingCart(book: BookResponseDto) {
    this.shoppingCartService.addItem(book);
  }
  navigateToDetails(book: BookResponseDto){
    this.router.navigate([`/book/${book.id}`]);
  }
}
