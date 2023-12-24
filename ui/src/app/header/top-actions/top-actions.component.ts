import { Component, OnInit } from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup'
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { Product } from '../../../domain/product';
import { ProductService } from '../../../services/productservice';
import { CarouselComponent } from '../../body/carousel/carousel.component';

@Component({
  selector: 'app-top-actions',
  standalone: true,
  imports: [InputGroupModule, ButtonModule, InputTextModule, OverlayPanelModule, CarouselComponent],
  templateUrl: './top-actions.component.html',
  styleUrl: './top-actions.component.css'
})
export class TopActionsComponent implements OnInit {
  products!: Product[];
  responsiveOptionsCartCarousel: any[] | undefined;

  constructor(private productService: ProductService) { }
  ngOnInit() {
    this.responsiveOptionsCartCarousel = [
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
    this.productService.getProductsMini().then((data) => {
      this.products = data;
    });
  }
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