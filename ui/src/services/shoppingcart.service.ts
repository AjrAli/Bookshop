import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CommonApiService } from "./common-api.service";
import { ShopItemResponseDto } from "../app/dto/shoppingcart/shopitem-response-dto";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { ToastService } from "./toast.service";
import { BookResponseDto } from "../app/dto/book/book-response-dto";
import { ShoppingCartDto } from "../app/dto/shoppingcart/shoppingcart-dto";
import { ShoppingCartResponse } from "../app/dto/handler-response/shoppingcart/shoppingcart-response";
import { ValidationErrorResponse } from "../app/dto/response/error/validation-error-response";

@Injectable()
export class ShoppingCartService extends CommonApiService {

    private shoppingCart: ShoppingCartResponseDto | null = null;
    private shoppingCartSubject: BehaviorSubject<ShoppingCartResponseDto | null> = new BehaviorSubject<ShoppingCartResponseDto | null>(null);

    constructor(http: HttpClient, private toastService: ToastService) {
        super(http);
        this.apiUrl += '/shopcart';
        this.loadShoppingCart();
    }
    private loadShoppingCart() {
        this.shoppingCart = this.getStoredShoppingCart();
        this.shoppingCartSubject.next(this.shoppingCart);
    }

    createShoppingCartToApi(shoppingCart: ShoppingCartDto) {
        this.http.post<ShoppingCartResponse>(`${this.apiUrl}/create-user-shopcart`, shoppingCart).subscribe({
            next: (r) => this.handleShoppingCartResponse(r),
            error: (e) => this.handleShoppingCartError(e),
            complete: () => console.info('complete')
        });
    }
    updateShoppingCartToApi(shoppingCart: ShoppingCartDto) {
        this.http.post<ShoppingCartResponse>(`${this.apiUrl}/update-user-shopcart`, shoppingCart).subscribe({
            next: (r) => this.handleShoppingCartResponse(r),
            error: (e) => this.handleShoppingCartError(e),
            complete: () => console.info('complete')
        });
    }

    private handleShoppingCartResponse(response: ShoppingCartResponse): void {
        if (response.shoppingCart) {
            this.setShoppingCart(response.shoppingCart);
            this.toastService.showSuccess(response.message);
        } else {
            this.toastService.showSimpleError('Invalid request');
        }
    }

    private handleShoppingCartError(error: any): void {
        this.toastService.showError(error.error as ValidationErrorResponse);
        this.toastService.showError(error as ValidationErrorResponse);
    }

    getStoredShoppingCart(): ShoppingCartResponseDto | null {
        const storedShoppingCart = localStorage.getItem('shoppingCart');
        if (!storedShoppingCart)
            return null;
        const instanceShoppingCart = new ShoppingCartResponseDto(JSON.parse(storedShoppingCart));
        return instanceShoppingCart;
    }
    private saveShoppingCart() {
        if (this.shoppingCart) {
            const serializedShoppingCart = JSON.stringify(this.shoppingCart);
            localStorage.setItem('shoppingCart', serializedShoppingCart);
            this.shoppingCartSubject.next(this.shoppingCart); // Notify subscribers about the update
        }
    }
    resetShoppingCart() {
        this.shoppingCart = null;
        this.shoppingCartSubject.next(this.shoppingCart);
        localStorage.removeItem('shoppingCart');
    }
    addItem(newItem: BookResponseDto) {
        let shopitem = new ShopItemResponseDto({ id: 0, quantity: 1, bookId: newItem.id, price: newItem.price * 1, title: newItem.title, imageUrl: newItem.imageUrl, authorName: newItem.authorName, categoryTitle: newItem.categoryTitle });
        if (this.shoppingCart) {
            const itemToUpdate = this.shoppingCart.items.find(item => item.bookId === shopitem.bookId);
            if (itemToUpdate) {
                const isQuantitySet = (itemToUpdate as ShopItemResponseDto).addQuantityWithLimit(1);
                if (!isQuantitySet) {
                    this.toastService.showSimpleError(`Limit quantity of 100 reached for ${itemToUpdate.title}`);
                    return;
                }
                itemToUpdate.price = itemToUpdate.quantity * newItem.price;
            } else {
                this.shoppingCart.items.push(shopitem);
            }
        } else {
            this.shoppingCart = new ShoppingCartResponseDto({ total: newItem.price, items: [shopitem] });
        }
        this.toastService.showSuccess(`Successfully added book in ShoppingCart`);
        this.shoppingCart.updateTotal();
        this.saveShoppingCart();
    }
    getShoppingCart(): ShoppingCartResponseDto | null {
        return this.shoppingCart;
    }
    updateShoppingCart(shoppingCart: ShoppingCartResponseDto) {
        if (!shoppingCart) {
            return;
        }
        if (!this.shoppingCart)
            this.shoppingCart = new ShoppingCartResponseDto();
        this.shoppingCart.updateItems(shoppingCart.items);
        this.shoppingCart.updateTotal();
        this.toastService.showSuccess(`Successfully updated your ShoppingCart`);
        this.saveShoppingCart();
    }
    setShoppingCart(shoppingCart: ShoppingCartResponseDto) {
        if (!shoppingCart) {
            return;
        }
        this.shoppingCart = new ShoppingCartResponseDto(shoppingCart);
        this.shoppingCart.updateTotal();
        this.toastService.showSuccess(`Successfully set ShoppingCart with valid data`);
        this.saveShoppingCart();
    }
    getShoppingCartObservable(): Observable<ShoppingCartResponseDto | null> {
        return this.shoppingCartSubject.asObservable().pipe(
            map((response: ShoppingCartResponseDto | null) => response ? new ShoppingCartResponseDto({ total: response.total, items: response.items }) : null)
        );
    }
}