import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common'
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';
import { Product } from '../../../domain/product';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-table',
  standalone: true,
  imports: [TableModule, TagModule, RatingModule, ButtonModule, FormsModule, CommonModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent {
  @Input() products!: Product[];
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
