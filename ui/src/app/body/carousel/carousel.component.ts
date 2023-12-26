import { Component, Input } from '@angular/core';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from "../../../app/environments/environment";
@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [CarouselModule, ButtonModule, TagModule],
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

  getSeverity(status: string) {
    switch (status) {
      case 'INSTOCK':
        return 'success';
      case 'LOWSTOCK':
        return 'warning';
      case 'OUTOFSTOCK':
        return 'danger';
    }
    return '';
  }
  getStockStatus(qt: number) {
    if (qt > 50)
      return 'INSTOCK';
    else if (qt < 50)
      return 'LOWSTOCK';
    else if (qt === 0)
      return 'OUTOFSTOCK';
    return '';
  }
}
