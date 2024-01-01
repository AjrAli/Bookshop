import { Injectable } from "@angular/core";
import { ShoppingCartResponseDto } from "../../app/dto/shoppingcart/shoppingcart-response-dto";

@Injectable()
export class ShoppingCartLocalStorageService {
    getStoredShoppingCart(): ShoppingCartResponseDto | null {
        // Retrieve and deserialize shopping cart from local storage
        const storedShoppingCart = localStorage.getItem('shoppingCart');
        return storedShoppingCart ? new ShoppingCartResponseDto(JSON.parse(storedShoppingCart)) : null;
    }

    storeShoppingCart(shoppingCart: ShoppingCartResponseDto | null) {
        // Serialize and save shopping cart to local storage
        if (shoppingCart) {
            const serializedShoppingCart = JSON.stringify(shoppingCart);
            localStorage.setItem('shoppingCart', serializedShoppingCart);
        }
    }
    removeStoredShoppingCart() {
        localStorage.removeItem('shoppingCart');
    }
}