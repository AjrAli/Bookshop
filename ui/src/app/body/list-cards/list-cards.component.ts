import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'
import { PaginatorModule } from 'primeng/paginator';
import { CardComponent } from '../../body/card/card.component';
import { BookResponseDto } from '../../dto/book/book-response-dto';

interface PageEvent {
  first: number;
  rows: number;
  page: number;
  pageCount: number;
}
@Component({
  selector: 'app-list-cards',
  standalone: true,
  imports: [PaginatorModule, CardComponent, CommonModule],
  templateUrl: './list-cards.component.html',
  styleUrl: './list-cards.component.css'
})
export class ListCardsComponent implements OnChanges {

  @Input() books!: BookResponseDto[];
  totalRecords: number = 0;
  rows: number = 8; // Number of items per page
  first: number = 0; // Initial page index
  ngOnChanges(): void {
    if (this.books.length > 0) {
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
