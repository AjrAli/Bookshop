import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { CommonModule } from '@angular/common';
import { ListCardsComponent } from '../../body/list-cards/list-cards.component';
import { BookService } from '../../../services/book.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PanelComponent } from '../../body/panel/panel.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule, ListCardsComponent, PanelComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css'
})
export class BooksComponent implements OnInit, OnDestroy {

  @Input() books: BookResponseDto[] | undefined;
  private booksSubscription: Subscription | undefined;
  private routeSubscription: Subscription | undefined;
  constructor(private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.booksSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe(params => {
      this.books = [];
      const id = Number.parseInt(params.get('id') ?? '');
      const routeType = this.route.snapshot.data['type'];

      if (routeType) {
        if (!Number.isNaN(id)) {
          if (routeType === 'author') {
            this.getBooksByAuthorId(id);
          } else if (routeType === 'category') {
            this.getBooksByCategoryId(id);
          }
        } else {
          this.resetBooks();
          this.router.navigate(['/books']);
        }
      } else {
        this.getAllBooks();
      }
    });
  }
  private getAllBooks(): void {
    this.booksSubscription = this.bookService.getAll().subscribe({
      next: (response) => this.handleBooksResponse(response),
      error: (error) => this.resetBooks()
    });
  }
  private getBooksByAuthorId(id: number): void {
    this.booksSubscription = this.bookService.getBooksByAuthorId(id).subscribe({
      next: (response) => this.handleBooksResponse(response),
      error: (error) => this.resetBooks()
    });
  }
  private getBooksByCategoryId(id: number): void {
    this.booksSubscription = this.bookService.getBooksByCategoryId(id).subscribe({
      next: (response) => this.handleBooksResponse(response),
      error: (error) => this.resetBooks()
    });
  }
  private handleBooksResponse(response: any): void {
    if (response && response.listDto) {
      this.books = response.listDto.map((book: BookResponseDto) => {
        return new BookResponseDto(book);
      })
    } else {
      this.books = undefined;
    }
  }
  private resetBooks() {
    this.books = undefined;
  }
}
