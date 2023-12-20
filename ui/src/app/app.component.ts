import { Component, OnInit, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { Product } from '../domain/product';
import { ProductService } from '../services/productservice';
import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputGroupModule } from 'primeng/inputgroup'
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, TabMenuModule, TableModule, TabViewModule,
            TagModule, RatingModule, FormsModule, ButtonModule, InputGroupModule,
            MenubarModule, InputTextModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent implements OnInit {
  products!: Product[];
  title = 'Bookshop';
  items: MenuItem[] | undefined;
  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.items = [
        {
            label: 'Home',
            icon: 'pi pi-fw pi-globe'
        },
        {
            label: 'Book1',
            icon: 'pi pi-fw pi-book'
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