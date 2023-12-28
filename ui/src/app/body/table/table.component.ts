import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common'
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from '../../environments/environment';



@Component({
  selector: 'app-table',
  standalone: true,
  imports: [TableModule, TagModule, RatingModule, ButtonModule, FormsModule, CommonModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent {
  rootUrl = environment.apiRootUrl;
  @Input() books!: BookResponseDto[];
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
