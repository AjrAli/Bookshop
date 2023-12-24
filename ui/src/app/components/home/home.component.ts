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



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ButtonModule, FormsModule,
    PanelComponent, FlyerPanelComponent, GalleriaComponent, CarouselComponent, 
    TableComponent, CardComponent, CommonModule],
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
}
