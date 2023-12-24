import { Component, OnInit } from '@angular/core';
import { GalleriaModule } from 'primeng/galleria';
import { PhotoService } from '../../../services/photoservice';
import { CarouselModule } from 'primeng/carousel';
import { Product } from '../../../domain/product';
import { ProductService } from '../../../services/productservice';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { TabMenuModule } from 'primeng/tabmenu';
import { RatingModule } from 'primeng/rating';
import { TabViewModule } from 'primeng/tabview';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [GalleriaModule, CarouselModule, TagModule, ButtonModule, CardModule,
            TableModule, TabMenuModule, RatingModule, TabViewModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  images: any[] | undefined;
  products!: Product[];
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
    private productService: ProductService) { }
  ngOnInit(): void {
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
