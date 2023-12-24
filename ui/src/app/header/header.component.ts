import { Component } from '@angular/core';
import { TopActionsComponent } from './top-actions/top-actions.component';
import { MenuComponent } from './menu/menu.component';
import { TopLinksComponent } from './top-links/top-links.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [TopActionsComponent, TopLinksComponent, MenuComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

}
