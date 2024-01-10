import { Component, Input } from '@angular/core';
import { ShopItemResponseDto } from '../../dto/shoppingcart/shopitem-response-dto';
import { CommonModule } from '@angular/common';
import { ShopItemComponent } from '../shop-item/shop-item.component';
import { ButtonModule } from 'primeng/button';
import { OverlayPanel } from 'primeng/overlaypanel';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-list-shop-items',
  standalone: true,
  imports: [CommonModule, ShopItemComponent, ButtonModule, CheckboxModule, FormsModule],
  templateUrl: './list-shop-items.component.html',
  styleUrl: './list-shop-items.component.css'
})
export class ListShopItemsComponent {
  @Input() items: ShopItemResponseDto[] | undefined;
  @Input() manage: string | null = null;
  shopIds: number[] = [];

  constructor() { }


}
