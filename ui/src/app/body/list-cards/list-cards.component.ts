import { Component, Input, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common'
import { PaginatorModule } from 'primeng/paginator';
import { CardComponent } from '../../body/card/card.component';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PageEvent } from './page-event';

@Component({
  selector: 'app-list-cards',
  standalone: true,
  imports: [PaginatorModule, CardComponent, CommonModule, ProgressSpinnerModule],
  templateUrl: './list-cards.component.html',
  styleUrl: './list-cards.component.css'
})
export class ListCardsComponent implements OnChanges {
  @Input() searchEngine = false;
  @Input() books: BookResponseDto[] | undefined;
  totalRecords: number = 0;
  rows: number = 10; // Number of items per page
  first: number = 0; // Initial page index
  ngOnChanges(): void {
    if (this.books && this.books.length > 0) {
      this.totalRecords = this.books.length;
    }
  }
  onPageChange(event: any): void {
    let eventOfPageEvent = event as PageEvent;
    if (eventOfPageEvent) {
      this.first = eventOfPageEvent.first;
      this.rows = eventOfPageEvent.rows;
    }
  }
}
