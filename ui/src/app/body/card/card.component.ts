import { Component, Input } from '@angular/core';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { environment } from '../../environments/environment';
import { TagModule } from 'primeng/tag';
@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CardModule, ButtonModule, TagModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  rootUrl = environment.apiRootUrl;
  @Input() book!: BookResponseDto;

}
