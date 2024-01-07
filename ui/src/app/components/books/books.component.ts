import { Component, Input, OnInit } from '@angular/core';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { CommonModule } from '@angular/common';
import { ListCardsComponent } from '../../body/list-cards/list-cards.component';
import { BookService } from '../../../services/book.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PanelComponent } from '../../body/panel/panel.component';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [CommonModule, ListCardsComponent, PanelComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css'
})
export class BooksComponent implements OnInit {

  @Input() books: BookResponseDto[] | undefined;
  constructor(private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
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
          this.router.navigate(['/books']);
        }
      } else {
        this.getAllBooks();
      }
    });
  }
  private getAllBooks(): void {
    this.bookService.getAll().subscribe({
      next: (response) => this.handleBooksResponse(response)
    });
  }
  private getBooksByAuthorId(id: number): void {
    this.bookService.getBooksByAuthorId(id).subscribe({
      next: (response) => this.handleBooksResponse(response)
    });
  }
  private getBooksByCategoryId(id: number): void {
    this.bookService.getBooksByCategoryId(id).subscribe({
      next: (response) => this.handleBooksResponse(response)
    });
  }
  private handleBooksResponse(response: any): void {
    if (response && response.listDto) {
      this.books = response.listDto.map((book: BookResponseDto) => {
        return new BookResponseDto(book);
      })
    }
  }
}
