import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { PanelComponent } from '../../body/panel/panel.component';
import { FlyerPanelComponent } from '../../body/flyer-panel/flyer-panel.component';
import { GalleriaComponent } from '../../body/galleria/galleria.component';
import { CarouselComponent } from '../../body/carousel/carousel.component';
import { ToastService } from '../../../services/toast.service';
import { BookService } from '../../../services/book.service';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { ErrorResponse } from '../../dto/response/error/error-response';
import { ListCardsComponent } from '../../body/list-cards/list-cards.component';
import { Subscription } from 'rxjs';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ButtonModule, FormsModule,
    PanelComponent, FlyerPanelComponent, GalleriaComponent, CarouselComponent, CommonModule, ListCardsComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {
  images: any[] | undefined;
  books: BookResponseDto[] | undefined = [];
  indexes: number[] = Array.from({ length: 10 }, (_, index) => index + 1);
  responsiveOptionsHomeCarousel: any[] | undefined;
  responsiveOptions: any[] = [
    {
      breakpoint: '1024px',
      numVisible: 5
    },
    {
      breakpoint: '768px',
      numVisible: 3
    },
    {
      breakpoint: '560px',
      numVisible: 1
    }
  ];
  private booksSubscription: Subscription | undefined;
  constructor(private toastService: ToastService,
    private bookService: BookService) { }

  ngOnDestroy(): void {
    this.booksSubscription?.unsubscribe();
  }

  getBooks() {
    this.booksSubscription = this.bookService.getAll().subscribe({
      next: (response: any) => {
        if (!response || response.listDto.length === 0) {
          this.books = undefined;
          return;
        }
        this.books = response.listDto.map((dto: any) => {
          return new BookResponseDto(dto);

        });
      },
      error: (error: ErrorResponse) => {
        this.books = undefined;
        this.toastService.showError(error);
      }
    })
  }
  ngOnInit(): void {
    this.getBooks();
    this.responsiveOptionsHomeCarousel = [
      {
        breakpoint: '1400px',
        numVisible: 3,
        numScroll: 3
      },
      {
        breakpoint: '1220px',
        numVisible: 2,
        numScroll: 2
      },
      {
        breakpoint: '1100px',
        numVisible: 1,
        numScroll: 1
      }
    ];
  }
}
