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
        const storedShoppingCart = sessionStorage.getItem('shoppingCart');
        this.shoppingCart = storedShoppingCart ? JSON.parse(storedShoppingCart) : null;
        this.shoppingCartSubject.next(this.shoppingCart);
    }

    private saveShoppingCart() {
        if (this.shoppingCart) {
            const serializedShoppingCart = JSON.stringify(this.shoppingCart);
            sessionStorage.setItem('shoppingCart', serializedShoppingCart);
            this.shoppingCartSubject.next(this.shoppingCart); // Notify subscribers about the update
        }
    }

    addItem(newItem: BookResponseDto) {
        let shopitem = new ShopItemResponseDto(0, 1, newItem.id, newItem.price * 1, newItem.title, newItem.imageUrl, newItem.authorName, newItem.categoryTitle);
        if (this.shoppingCart) {
            const itemToUpdate = this.shoppingCart.items.find(item => item.bookId === shopitem.bookId);
            if (itemToUpdate) {
                if (itemToUpdate.quantity < 100)
                    itemToUpdate.quantity += 1;
                else
                    this.toastService.showSimpleError(`Limit quantity of 100 reached for ${shopitem.title}`);
            } else {
                this.shoppingCart.items.push(shopitem);
            }
        } else {
            this.shoppingCart = new ShoppingCartResponseDto(newItem.price, [shopitem]);
        }
        this.saveShoppingCart();
    }

    getShoppingCart(): ShoppingCartResponseDto | null {
        return this.shoppingCart;
    }

    getShoppingCartObservable(): Observable<ShoppingCartResponseDto | null> {
        return this.shoppingCartSubject.asObservable().pipe(
            map((response: ShoppingCartResponseDto | null) => response ? new ShoppingCartResponseDto(response.total, response.items) : null)
        );
    }
}