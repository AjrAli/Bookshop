import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { ShoppingCartResponseDto } from "../../app/dto/shoppingcart/shoppingcart-response-dto";
import { map } from 'rxjs/operators';

@Injectable()
export class ShoppingCartDataService {
    private shoppingCart: ShoppingCartResponseDto | null = null;
    private shoppingCartSubject: BehaviorSubject<ShoppingCartResponseDto | null> = new BehaviorSubject<ShoppingCartResponseDto | null>(null);

    getShoppingCart(): ShoppingCartResponseDto | null {
        return this.shoppingCart;
    }
    getShoppingCartObservable(): Observable<ShoppingCartResponseDto | null> {
        return this.shoppingCartSubject.asObservable().pipe(
            map((response: ShoppingCartResponseDto | null) => response ? new ShoppingCartResponseDto({ total: response.total, items: response.items }) : null)
        );
    }
    updateShoppingCart(shoppingCart: ShoppingCartResponseDto) {
        if (!shoppingCart) {
            return;
        }
        if (!this.shoppingCart)
            this.shoppingCart = new ShoppingCartResponseDto();
        this.shoppingCart.updateItems(shoppingCart.items);
        this.shoppingCart.updateTotal();
        this.shoppingCartSubject.next(this.shoppingCart);
    }
    setShoppingCart(shoppingCart: ShoppingCartResponseDto) {
        if (!shoppingCart) {
            return;
        }
        this.shoppingCart = new ShoppingCartResponseDto(shoppingCart);
        this.shoppingCart.updateTotal();
        this.shoppingCartSubject.next(this.shoppingCart);
    }
    resetShoppingCart(){
        this.shoppingCart = null;
        this.shoppingCartSubject.next(this.shoppingCart);
    }

}