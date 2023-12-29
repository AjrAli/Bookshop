import { Component, Input } from '@angular/core';
import { environment } from '../../environments/environment';
import { ShopItemResponseDto } from '../../dto/shoppingcart/shopitem-response-dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-shop-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './shop-item.component.html',
  styleUrl: './shop-item.component.css'
})
export class ShopItemComponent {

  rootUrl = environment.apiRootUrl;
  @Input() shopItem!: ShopItemResponseDto;
}
