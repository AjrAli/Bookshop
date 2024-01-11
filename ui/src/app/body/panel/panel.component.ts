import { Component, OnDestroy, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { PanelMenuModule } from 'primeng/panelmenu';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';
import { AuthorService } from '../../../services/author.service';
import { AuthorResponseDto } from '../../dto/author/author-response-dto';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-panel',
  standalone: true,
  imports: [PanelModule, PanelMenuModule, CommonModule],
  templateUrl: './panel.component.html',
  styleUrl: './panel.component.css'
})
export class PanelComponent implements OnInit, OnDestroy {
  itemsPanelMenu: MenuItem[] | undefined;
  authors: AuthorResponseDto[] | undefined;
  private authorsSubscription: Subscription | undefined;
  constructor(private router: Router, private authorService: AuthorService) { }

  ngOnDestroy(): void {
    this.authorsSubscription?.unsubscribe();
  }

  ngOnInit() {
    this.authorsSubscription = this.authorService.getAll().subscribe({
      next: (r) => {
        if (r && r.listDto) {
          this.authors = r.listDto;
          const items: { label: string | undefined, icone: string | undefined, command: any }[] = [];
          for (let autor of this.authors) {
            items.push({
              label: autor.name,
              icone: 'pi pi-fw pi-user',
              command: () => this.router.navigate([`/books/author/${autor.id}`])
            })
          }
          this.itemsPanelMenu = [
            {
              label: 'By Authors',
              icon: 'pi pi-fw pi-user',
              expanded: true,
              items: items
            }
          ];
        }
      }
    });
  }
}
