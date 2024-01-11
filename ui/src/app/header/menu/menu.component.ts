import { Component, OnDestroy, OnInit } from '@angular/core';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';
import { CategoryResponseDto } from '../../dto/category/category-response-dto';
import { CategoryService } from '../../../services/category.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [MenubarModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit, OnDestroy {
  items: MenuItem[] | undefined;
  categories: CategoryResponseDto[] | undefined;
  private categorySubscription: Subscription | undefined;
  constructor(private router: Router, private categoryService: CategoryService) { }

  ngOnDestroy(): void {
    this.categorySubscription?.unsubscribe();
  }
  ngOnInit() {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-fw pi-globe',
        command: () => this.router.navigate([''])
      }
    ];
    this.categorySubscription = this.categoryService.getAll().subscribe({
      next: (r) => {
        if (r && r.listDto) {
          this.categories = r.listDto;
          this.items = [
            {
              label: 'Home',
              icon: 'pi pi-fw pi-globe',
              command: () => this.router.navigate([''])
            },
            {
              label: 'All Books',
              icon: 'pi pi-fw pi-book',
              command: () => this.router.navigate(['/books'])
            }
          ];
          for (let category of this.categories) {
            this.items?.push({
              label: category.title,
              icone: 'pi pi-fw pi-book',
              command: () => this.router.navigate([`/books/category/${category.id}`])
            });
          }
        }
      }
    });
  }
}
