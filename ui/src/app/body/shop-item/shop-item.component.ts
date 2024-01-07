import { Component, Input, OnInit } from '@angular/core';
import { environment } from '../../environments/environment';
import { ShopItemResponseDto } from '../../dto/shoppingcart/shopitem-response-dto';
import { CommonModule } from '@angular/common';
import { ShoppingCartService } from '../../../services/shoppingcart.service';
import { DropdownModule } from 'primeng/dropdown';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { FormValidationErrorComponent } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';

@Component({
  selector: 'app-shop-item',
  standalone: true,
  imports: [CommonModule, DropdownModule, ReactiveFormsModule, FormsModule, FormValidationErrorComponent, ButtonModule],
  templateUrl: './shop-item.component.html',
  styleUrl: './shop-item.component.css'
})
export class ShopItemComponent implements OnInit {

  rootUrl = environment.apiRootUrl;
  @Input() shopItem: ShopItemResponseDto | undefined;
  @Input() manage: string | null = null;
  quantityForm!: FormGroup;
  quantity: number = 0;
  quantityOptions: { label: string, value: number }[] = [];

  constructor(private shoppingCartService: ShoppingCartService, private router: Router) { }
  ngOnInit(): void {
    this.quantityForm = new FormGroup({
      quantity: new FormControl(this.quantity, [Validators.required, Validators.max(100)])
    });
    if (this.shopItem) {
      this.quantityOptions = Array.from({ length: this.shopItem.MAX_QUANTITY }, (_, i) => ({ label: (i + 1).toString(), value: i + 1 }));
      this.quantityForm.get('quantity')?.setValue(this.shopItem?.quantity);
      this.quantityForm.get('quantity')?.valueChanges.subscribe({
        next: (value: number) => {
          if (this.shopItem && value > 0 && value <= this.shopItem.MAX_QUANTITY) {
            this.shopItem.quantity = value;
            this.shoppingCartService.updateItem(this.shopItem);
          }
        }
      })
    }
  }
  navigateToDetails(id: number) {
    this.router.navigate([`/book/${id}`]);
  }
  removeItem() {
    if (this.shopItem)
      this.shoppingCartService.removeItem(this.shopItem);
  }

}
