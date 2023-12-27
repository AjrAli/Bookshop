import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'
import { PhotoService } from '../../../services/photoservice';
import { Product } from '../../../domain/product';
import { ProductService } from '../../../services/productservice';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { PanelComponent } from '../../body/panel/panel.component';
import { FlyerPanelComponent } from '../../body/flyer-panel/flyer-panel.component';
import { GalleriaComponent } from '../../body/galleria/galleria.component';
import { CarouselComponent } from '../../body/carousel/carousel.component';
import { TableComponent } from '../../body/table/table.component';
import { CardComponent } from '../../body/card/card.component';
import { ToastService } from '../../../services/toast.service';
import { BookService } from '../../../services/book.service';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { ErrorResponse } from '../../dto/response/error/error-response';
import { PaginatorModule } from 'primeng/paginator';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ButtonModule, FormsModule,
    PanelComponent, FlyerPanelComponent, GalleriaComponent, CarouselComponent,
    TableComponent, CardComponent, CommonModule, PaginatorModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  totalRecords: number = 0;
  rows: number = 8; // Number of items per page
  first: number = 0; // Initial page index
  images: any[] | undefined;
  products!: Product[];
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
  constructor(private photoService: PhotoService,
    private productService: ProductService,
    private toastService: ToastService,
    private bookService: BookService) { }

  onPageChange(event: any): void {
    this.first = event.first;
  }
  getBooks() {
    this.bookService.getAll().subscribe({
      next: (response: any) => {
        if (!response || response.listDto.length === 0) {
          this.books = undefined;
          return;
        }
        this.books = response.listDto.map((listDto: any) => {
          const book = new BookResponseDto();
          Object.assign(book, listDto);
          return book;
        });
        this.totalRecords = this.books!.length;
      },
      error: (error: ErrorResponse) => {
        this.books = undefined;
        this.toastService.showError(error);
      },
      complete: () => console.info('complete')
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
    this.photoService.getImages().then(images => {
      this.images = images;
    });
    this.productService.getProductsMini().then((data) => {
      this.products = data;
    });
  }
}
