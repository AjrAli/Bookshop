import { Injectable } from "@angular/core";
import { ShopItemResponseDto } from "../app/dto/shoppingcart/shopitem-response-dto";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { ToastService } from "./toast.service";
import { BookResponseDto } from "../app/dto/book/book-response-dto";
import { ShoppingCartDto } from "../app/dto/shoppingcart/shoppingcart-dto";
import { ShoppingCartApiService } from "./shoppingcart/shoppingcart-api.service";
import { ShoppingCartLocalStorageService } from "./shoppingcart/shoppingcart-local-storage.service";
import { ShoppingCartDataService } from "./shoppingcart/shoppingcart-data.service";
import { Observable, map, tap } from "rxjs";
import { ShoppingCartCommandResponse } from "../app/dto/handler-response/shoppingcart/shoppingcart-command-response";

@Injectable()
export class ShoppingCartService {
    constructor(private shoppingcartApiService: ShoppingCartApiService,
        private shoppingCartLocalStorageService: ShoppingCartLocalStorageService,
        private shoppingCartDataService: ShoppingCartDataService,
        private toastService: ToastService,
    ) {
        this.loadShoppingCart();
    }
    private loadShoppingCart() {
        const shoppingCart = this.shoppingCartLocalStorageService.getStoredShoppingCart();
        if (shoppingCart)
            this.shoppingCartDataService.setShoppingCart(shoppingCart);
    }
    createShoppingCartFromApi(shoppingCart: ShoppingCartDto): Observable<ShoppingCartResponseDto> {
        return this.shoppingcartApiService.createShoppingCart(shoppingCart).pipe(tap({
            next: (r) => this.handleShoppingCartResponse(r),
            error: (e) => this.handleShoppingCartError(e),
            complete: () => console.info('complete')
        }), map(response => response.shoppingCart));
    }
    updateShoppingCartFromApi(shoppingCart: ShoppingCartDto): Observable<ShoppingCartResponseDto> {
        return this.shoppingcartApiService.updateShoppingCart(shoppingCart).pipe(tap({
            next: (r) => this.handleShoppingCartResponse(r),
            error: (e) => this.handleShoppingCartError(e),
            complete: () => console.info('complete')
        }), map(response => response.shoppingCart));
    }
    resetShoppingCartFromApi(): Observable<boolean> {
        return this.shoppingcartApiService.resetShoppingCart().pipe(tap({
            next: (r) => this.toastService.showSuccess(r.message),
            error: (e) => this.handleShoppingCartError(e),
            complete: () => console.info('complete')
        }), map(response => !!response.success));
    }

    private handleShoppingCartResponse(response: ShoppingCartCommandResponse): void {
        if (response.shoppingCart) {
            this.shoppingCartDataService.setShoppingCart(response.shoppingCart);
            this.toastService.showSuccess(response.message);
        } else {
            this.toastService.showValidationError(response);
        }
    }

    private handleShoppingCartError(error: any): void {
        this.toastService.showError(error.error);
        this.toastService.showError(error);
    }
    updateFullyShoppingCart(shoppingCart: ShoppingCartResponseDto | null, message?: string) {
        if (shoppingCart) {
            this.shoppingCartLocalStorageService.storeShoppingCart(shoppingCart);
            this.shoppingCartDataService.updateShoppingCart(shoppingCart);
            if (message)
                this.toastService.showSuccess(message);
        }
    }
    private saveFullyShoppingCart(shoppingCart: ShoppingCartResponseDto | null, message?: string) {
        if (shoppingCart) {
            this.shoppingCartLocalStorageService.storeShoppingCart(shoppingCart);
            this.shoppingCartDataService.setShoppingCart(shoppingCart);
            this.toastService.showSuccess(message ?? `Successfully updated your ShoppingCart`);
        }
    }
    resetLocalShoppingCart() {
        this.shoppingCartDataService.resetShoppingCart();
        this.shoppingCartLocalStorageService.removeStoredShoppingCart();
    }
    addItem(newItem: BookResponseDto) {
        let shopitem = new ShopItemResponseDto({ id: 0, quantity: 1, bookId: newItem.id, price: newItem.price * 1, bookPrice: newItem.price, title: newItem.title, imageUrl: newItem.imageUrl, authorName: newItem.authorName, categoryTitle: newItem.categoryTitle });
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
        this.saveFullyShoppingCart(shoppingCart, "item added to shoppingcart");
    }
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
                this.toastService.showSimpleError(`Book ${shopitem.title} not found in ShoppingCart`);
                return;
            }
        }
    }
    removeItem(shopitem: ShopItemResponseDto) {
        const shoppingCart = this.shoppingCartDataService.getShoppingCart();
        if (!shoppingCart) {
            return;
        }
        const itemToDeleteIndex = shoppingCart.items.findIndex(item => item.bookId === shopitem.bookId);
        if (itemToDeleteIndex === -1) {
            this.toastService.showSimpleError(`Book ${shopitem.title} not found in ShoppingCart`);
            return;
        }
        shoppingCart.items.splice(itemToDeleteIndex, 1);
        this.saveFullyShoppingCart(shoppingCart, "item correctly removed");
    }
}