import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ShoppingCartResponse } from "../../app/dto/handler-response/shoppingcart/shoppingcart-response";
import { ShoppingCartDto } from "../../app/dto/shoppingcart/shoppingcart-dto";
import { CommonApiService } from "../common-api.service";
import { BaseResponse } from "../../app/dto/response/base-response";

@Injectable()
export class ShoppingCartApiService extends CommonApiService {
    constructor(http: HttpClient) {
        super(http);
        this.apiUrl += '/shopcart';
    }

    createShoppingCart(shoppingCart: ShoppingCartDto): Observable<ShoppingCartResponse> {
        // API call to create a new shopping cart
        return this.http.post<ShoppingCartResponse>(`${this.apiUrl}/create-user-shopcart`, shoppingCart);
    }

    updateShoppingCart(shoppingCart: ShoppingCartDto): Observable<ShoppingCartResponse> {
        // API call to update an existing shopping cart
        return this.http.post<ShoppingCartResponse>(`${this.apiUrl}/update-user-shopcart`, shoppingCart);
    }

    resetShoppingCart(): Observable<BaseResponse> {
        // API call to update an existing shopping cart
        return this.http.post<BaseResponse>(`${this.apiUrl}/reset-user-shopcart`, {});
    }

}