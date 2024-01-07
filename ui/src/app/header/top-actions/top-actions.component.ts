import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup'
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';
import { CustomerService } from '../../../services/customer.service';
import { ShoppingCartResponseDto } from '../../dto/shoppingcart/shoppingcart-response-dto';
import { BookService } from '../../../services/book.service';
import { ListShopItemsComponent } from '../../body/list-shop-items/list-shop-items.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ShoppingCartDataService } from '../../../services/shoppingcart/shoppingcart-data.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SearchStateService } from '../../../services/search/search-state.service';

@Component({
  selector: 'app-top-actions',
  standalone: true,
  imports: [InputGroupModule, ButtonModule, InputTextModule, OverlayPanelModule, ScrollPanelModule, CommonModule, ListShopItemsComponent, ReactiveFormsModule],
  templateUrl: './top-actions.component.html',
  styleUrl: './top-actions.component.css'
})
export class TopActionsComponent implements OnInit, OnDestroy {
  searchForm!: FormGroup;
  searchField: string = '';
  rootUrl = environment.apiRootUrl;
  private shoppingCartSubscription: Subscription | undefined;
  shoppingcart: ShoppingCartResponseDto | null = new ShoppingCartResponseDto();
  @ViewChild('op') op!: OverlayPanel;
  constructor(private shoppingCartDataService: ShoppingCartDataService,
    private searchStateService: SearchStateService,
    private router: Router) { }
  ngOnDestroy(): void {
    if (this.shoppingCartSubscription)
      this.shoppingCartSubscription.unsubscribe();
  }

  ngOnInit() {
    this.searchForm = new FormGroup({
      searchField: new FormControl(this.searchField)
    });
    this.shoppingCartSubscription = this.shoppingCartDataService.getShoppingCartObservable().subscribe({
      next: (response: ShoppingCartResponseDto | null) => {
        if (!response || response.items.length === 0) {
          this.shoppingcart = null;
          return;
        }
        this.shoppingcart = response;
      }
    })
  }
  submitSearch(value?: string) {

    if (value) {
      this.searchField = value
      this.router.navigate(['/search'], { queryParams: { keyword: this.searchField } });
      this.searchStateService.setSearchKeyword(this.searchField);
    } else {
      this.router.navigate(['/search'], { queryParams: { keyword: this.searchForm.value.searchField } });
      this.searchStateService.setSearchKeyword(this.searchForm.value.searchField);
    }
  }
  getTotalItems() {
    return this.shoppingcart?.getTotalItems() ?? 0;
  }
  goToShoppingCart(event: any) {
    if (this.op) {
      this.op.toggle(event);
      this.router.navigate(['/steps']);
    }
  }
}

