import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CommonApiService } from "./common-api.service";
import { ShopItemResponseDto } from "../app/dto/shoppingcart/shopitem-response-dto";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { ToastService } from "./toast.service";
import { BookResponseDto } from "../app/dto/book/book-response-dto";

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
    getStoredShoppingCart(): ShoppingCartResponseDto | null {
        const storedShoppingCart = sessionStorage.getItem('shoppingCart');
        if (!storedShoppingCart)
            return null;
        const instanceShoppingCart = new ShoppingCartResponseDto(JSON.parse(storedShoppingCart));
        return instanceShoppingCart;
    }
    private saveShoppingCart() {
        if (this.shoppingCart) {
            const serializedShoppingCart = JSON.stringify(this.shoppingCart);
            sessionStorage.setItem('shoppingCart', serializedShoppingCart);
            this.shoppingCartSubject.next(this.shoppingCart); // Notify subscribers about the update
        }
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
    setShoppingCart(shoppingCart: ShoppingCartResponseDto) {
        if (!shoppingCart) {
            return;
        }
        if (!this.shoppingCart)
            this.shoppingCart = new ShoppingCartResponseDto();
        this.shoppingCart.updateItems(shoppingCart.items);
        this.shoppingCart.updateTotal();
        this.toastService.showSuccess(`Successfully loaded your previous ShoppingCart`);
        this.saveShoppingCart();
    }
    getShoppingCartObservable(): Observable<ShoppingCartResponseDto | null> {
        return this.shoppingCartSubject.asObservable().pipe(
            map((response: ShoppingCartResponseDto | null) => response ? new ShoppingCartResponseDto({ total: response.total, items: response.items }) : null)
        );
    }
}