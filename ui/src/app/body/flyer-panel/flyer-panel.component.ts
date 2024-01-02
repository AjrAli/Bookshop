import { Component } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-flyer-panel',
  standalone: true,
  imports: [],
  templateUrl: './flyer-panel.component.html',
  styleUrl: './flyer-panel.component.css'
})
export class FlyerPanelComponent {
  rootUrl = environment.apiRootUrl;
  img1 = '/client/img/image-book1.jpg';
  img2 = '/client/img/image-book2.jpg';
  img3 = '/client/img/image-book3.jpg';
}
