import { Component, Input } from '@angular/core';
import { ShopItemResponseDto } from '../../dto/shoppingcart/shopitem-response-dto';
import { CommonModule } from '@angular/common';
import { ShopItemComponent } from '../shop-item/shop-item.component';

@Component({
  selector: 'app-list-shop-items',
  standalone: true,
  imports: [CommonModule, ShopItemComponent],
  templateUrl: './list-shop-items.component.html',
  styleUrl: './list-shop-items.component.css'
})
export class ListShopItemsComponent {
  @Input() items!: ShopItemResponseDto[];
}
