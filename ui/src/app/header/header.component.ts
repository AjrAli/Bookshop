import { Component } from '@angular/core';
import { TopActionsComponent } from './top-actions/top-actions.component';
import { MenuComponent } from './menu/menu.component';
import { TopLinksComponent } from './top-links/top-links.component';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [TopActionsComponent, TopLinksComponent, MenuComponent, ToastModule, DialogModule, ButtonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  visible: boolean = true;
  redirectToGithub() {
    window.location.href = 'https://github.com/AjrAli/Bookshop';
  }
}
