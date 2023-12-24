import { Component, Input } from '@angular/core';
import { GalleriaModule } from 'primeng/galleria';

@Component({
  selector: 'app-galleria',
  standalone: true,
  imports: [GalleriaModule],
  templateUrl: './galleria.component.html',
  styleUrl: './galleria.component.css'
})
export class GalleriaComponent {
  @Input() images: any[] | undefined;
  @Input() responsiveOptions: any[] | undefined;

}
