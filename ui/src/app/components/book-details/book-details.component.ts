import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../../services/book.service';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { ToastService } from '../../../services/toast.service';
import { ErrorResponse } from '../../dto/response/error/error-response';
import { environment } from '../../environments/environment';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';
import { ShoppingCartService } from '../../../services/shoppingcart.service';
import { ImageModule } from 'primeng/image';
import { Subscription } from 'rxjs';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule, DividerModule, ButtonModule, ImageModule, RatingModule, FormsModule],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css'
})
export class BookDetailsComponent implements OnInit, OnDestroy {

  book: BookResponseDto | null = null;
  rootUrl = environment.apiRootUrl;
  private bookSubscription: Subscription | undefined;
  constructor(private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private shoppingCartService: ShoppingCartService,
    private toastService: ToastService) { }
  ngOnDestroy(): void {
    this.bookSubscription?.unsubscribe();
  }
  ngOnInit(): void {
    const id = Number.parseInt(this.route.snapshot.params['id']);
    if (!Number.isNaN(id)) {
      this.bookSubscription = this.bookService.getById(id).subscribe({
        next: (r) => {
          if (r.dto) {
            this.book = new BookResponseDto(r.dto);
          } else {
            this.backToIndex();
          }
        },
        error: (e: ErrorResponse) => {
          this.backToIndex();
          this.toastService.showError(e);
        }
      })
    } else {
      this.backToIndex();
    }
  }
  backToIndex() {
    this.router.navigate(['']);
  }
  sendToShoppingCart(book: BookResponseDto) {
    this.shoppingCartService.addItem(book);
  }

}
