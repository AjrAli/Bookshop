import { Injectable } from "@angular/core";
import { ShopItemResponseDto } from "../app/dto/shoppingcart/shopitem-response-dto";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { ToastService } from "./toast.service";
import { BookResponseDto } from "../app/dto/book/book-response-dto";
import { ShoppingCartDto } from "../app/dto/shoppingcart/shoppingcart-dto";
import { ShoppingCartApiService } from "./shoppingcart/shoppingcart-api.service";
import { ShoppingCartLocalStorageService } from "./shoppingcart/shoppingcart-local-storage.service";
import { ShoppingCartDataService } from "./shoppingcart/shoppingcart-data.service";
import { Observable, Subscription, map, tap } from "rxjs";
import { ShoppingCartCommandResponse } from "../app/dto/handler-response/shoppingcart/shoppingcart-command-response";

@Injectable()
export class ShoppingCartService {
  constructor(
    private shoppingCartApiService: ShoppingCartApiService,
    private shoppingCartLocalStorageService: ShoppingCartLocalStorageService,
    private shoppingCartDataService: ShoppingCartDataService,
    private toastService: ToastService,
  ) {
    this.loadShoppingCart();
  }

  // Load shopping cart from local storage
  private loadShoppingCart() {
    const shoppingCart = this.shoppingCartLocalStorageService.getStoredShoppingCart();
    if (shoppingCart) {
      this.shoppingCartDataService.setShoppingCart(shoppingCart);
    }
  }
  // Set an available shoppingcart as default
  setAvailableShoppingCartFromApi(): Observable<ShoppingCartResponseDto> {
    return this.shoppingCartApiService.getShoppingCart().pipe(
      tap({
        next: (shopResponse) => {
          if (shopResponse && shopResponse.shoppingCart?.items.length > 0) {
            this.updateFullyShoppingCart(new ShoppingCartResponseDto(shopResponse.shoppingCart));
          }
        },
        error: (e) => {
          this.toastService.showError(e);
        }
      }),
      map(shopResponse => shopResponse.shoppingCart)
    );
  }
  // Create a new shopping cart on the server
  createShoppingCartFromApi(shoppingCart: ShoppingCartDto): Observable<ShoppingCartResponseDto> {
    return this.shoppingCartApiService.createShoppingCart(shoppingCart).pipe(
      tap({
        next: (response) => this.handleShoppingCartResponse(response),
        error: (error) => this.handleShoppingCartError(error)
      }),
      map(response => response.shoppingCart)
    );
  }

  // Update an existing shopping cart on the server
  updateShoppingCartFromApi(shoppingCart: ShoppingCartDto): Observable<ShoppingCartResponseDto> {
    return this.shoppingCartApiService.updateShoppingCart(shoppingCart).pipe(
      tap({
        next: (response) => this.handleShoppingCartResponse(response),
        error: (error) => this.handleShoppingCartError(error)
      }),
      map(response => response.shoppingCart)
    );
  }

  // Reset the shopping cart on the server
  resetShoppingCartFromApi(): Observable<boolean> {
    return this.shoppingCartApiService.resetShoppingCart().pipe(
      tap({
        next: (response) => this.toastService.showSuccess(response.message),
        error: (error) => this.handleShoppingCartError(error)
      }),
      map(response => !!response.success)
    );
  }
  // Handle response from shopping cart command (create, update, reset)
  private handleShoppingCartResponse(response: ShoppingCartCommandResponse): void {
    if (response.shoppingCart) {
      this.shoppingCartDataService.setShoppingCart(response.shoppingCart);
      this.toastService.showSuccess(response.message);
    } else {
      this.toastService.showValidationError(response);
    }
  }

  // Handle shopping cart command error
  private handleShoppingCartError(error: any): void {
    this.toastService.showError(error.error);
    this.toastService.showError(error);
  }

  // Update local and server shopping cart details
  updateFullyShoppingCart(shoppingCart: ShoppingCartResponseDto | undefined, message?: string) {
    if (shoppingCart) {
      this.shoppingCartLocalStorageService.storeShoppingCart(shoppingCart);
      this.shoppingCartDataService.updateShoppingCart(shoppingCart);
      if (message) {
        this.toastService.showSuccess(message);
      }
    }
  }

  // Save fully updated shopping cart details locally and on the server
  private saveFullyShoppingCart(shoppingCart: ShoppingCartResponseDto | undefined, message?: string) {
    if (shoppingCart) {
      this.shoppingCartLocalStorageService.storeShoppingCart(shoppingCart);
      this.shoppingCartDataService.setShoppingCart(shoppingCart);
      this.toastService.showSuccess(message ?? `Successfully updated your shopping cart`);
    }
  }

  // Reset local shopping cart data
  resetLocalShoppingCart() {
    this.shoppingCartDataService.resetShoppingCart();
    this.shoppingCartLocalStorageService.removeStoredShoppingCart();
  }

  // Add a new item to the shopping cart
  addItem(newItem: BookResponseDto) {
    let shopitem = new ShopItemResponseDto({
      quantity: 1,
      bookId: newItem.id,
      price: newItem.price,
      bookPrice: newItem.price,
      title: newItem.title,
      imageUrl: newItem.imageUrl,
      authorName: newItem.authorName,
      categoryTitle: newItem.categoryTitle
    });

    let shoppingCart = this.shoppingCartDataService.getShoppingCart();
    if (shoppingCart) {
      const itemToUpdate = shoppingCart.items.find(item => item.bookId === shopitem.bookId);
      if (itemToUpdate) {
        const isQuantitySet = itemToUpdate.addQuantityWithLimit(1);
        if (!isQuantitySet) {
          this.toastService.showSimpleError(`Limit quantity of 100 reached for ${itemToUpdate.title}`);
          return;
        }
        itemToUpdate.price = itemToUpdate.quantity * newItem.price;
      } else {
        shoppingCart.items.push(shopitem);
      }
    } else {
      shoppingCart = new ShoppingCartResponseDto({ total: newItem.price, items: [shopitem] });
    }
    this.saveFullyShoppingCart(shoppingCart, "Item added to shopping cart");
  }

  // Update the quantity of an item in the shopping cart
  updateItem(shopitem: ShopItemResponseDto) {
    let shoppingCart = this.shoppingCartDataService.getShoppingCart();
    if (shoppingCart) {
      const itemToUpdate = shoppingCart.items.find(item => item.bookId === shopitem.bookId);
      if (itemToUpdate) {
        shopitem.setValidQuantity(shopitem.quantity);
        itemToUpdate.quantity = shopitem.quantity;
        itemToUpdate.price = itemToUpdate.quantity * shopitem.bookPrice;
        this.saveFullyShoppingCart(shoppingCart);
      } else {
        this.toastService.showSimpleError(`Book ${shopitem.title} not found in shopping cart`);
        return;
      }
    }
  }

  // Remove an item from the shopping cart
  removeItem(shopitem: ShopItemResponseDto) {
    const shoppingCart = this.shoppingCartDataService.getShoppingCart();
    if (!shoppingCart) {
      return;
    }
    const itemToDeleteIndex = shoppingCart.items.findIndex(item => item.bookId === shopitem.bookId);
    if (itemToDeleteIndex === -1) {
      this.toastService.showSimpleError(`Book ${shopitem.title} not found in shopping cart`);
      return;
    }
    shoppingCart.items.splice(itemToDeleteIndex, 1);
    this.saveFullyShoppingCart(shoppingCart, "Item correctly removed");
  }
}
