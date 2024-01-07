import { Component, Input, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from '../../environments/environment';
import { TagModule } from 'primeng/tag';
import { ShoppingCartService } from '../../../services/shoppingcart.service';
import { Router } from '@angular/router';
import { ImageModule } from 'primeng/image';
import { Observable } from 'rxjs';
import { SearchStateService } from '../../../services/search/search-state.service';
import { BoldTextPipe } from '../../components/search/bold-text.pipe';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-card',
  standalone: true,
  imports: [ButtonModule, TagModule, ImageModule, BoldTextPipe, CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent implements OnInit {
  keyword$!: Observable<string> | null;
  rootUrl = environment.apiRootUrl;
  @Input() book!: BookResponseDto;
  @Input() searchEngine = false;

  constructor(private shoppingCartService: ShoppingCartService,
              private searchStateService: SearchStateService,
              private router: Router) { }

  ngOnInit(): void {
    if (this.searchEngine) {
      this.keyword$ = this.searchStateService.searchKeyword$;
    }
  }


  sendToShoppingCart(book: BookResponseDto) {
    this.shoppingCartService.addItem(book);
  }

  navigateToDetails(book: BookResponseDto){
    this.router.navigate([`/book/${book.id}`]);
  }

}
