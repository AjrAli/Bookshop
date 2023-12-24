import { Component, Input } from '@angular/core';
import { CarouselModule } from 'primeng/carousel';
import { Product } from '../../../domain/product';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
@Component({
  selector: 'app-carousel',
  standalone: true,
  imports: [CarouselModule, ButtonModule, TagModule],
  templateUrl: './carousel.component.html',
  styleUrl: './carousel.component.css'
})
export class CarouselComponent {
  @Input() verticalViewPortHeight!: string;
  @Input() orientation!: any;
  @Input() numVisible!: number;
  @Input() products!: Product[];
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
}
