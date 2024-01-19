import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ShoppingCartDto } from "../../app/dto/shoppingcart/shoppingcart-dto";
import { BaseResponse } from "../../app/dto/response/base-response";
import { ShoppingCartDetailsResponse } from "../../app/dto/handler-response/shoppingcart/shoppingcart-details-response";
import { ShoppingCartCommandResponse } from "../../app/dto/handler-response/shoppingcart/shoppingcart-command-response";
import { environment } from "../../app/environments/environment";
import { ShoppingCartResponse } from "../../app/dto/handler-response/shoppingcart/shoppingcart-response";

@Injectable()
export class ShoppingCartApiService {
    protected apiUrl: string = environment.apiUrl + '/shopcart';

    constructor(private http: HttpClient) { }

    getShoppingCart(): Observable<ShoppingCartResponse> {
        // API call to get a full details of shopping cart
        return this.http.get<ShoppingCartResponse>(`${this.apiUrl}/get-user-shopcart`);
    }

    getShoppingCartDetails(): Observable<ShoppingCartDetailsResponse> {
        // API call to get a full details of shopping cart
        return this.http.get<ShoppingCartDetailsResponse>(`${this.apiUrl}/get-user-shopcart-details-review`);
    }

    createShoppingCart(shoppingCart: ShoppingCartDto): Observable<ShoppingCartCommandResponse> {
        // API call to create a new shopping cart
        return this.http.post<ShoppingCartCommandResponse>(`${this.apiUrl}/create-user-shopcart`, shoppingCart);
    }

    updateShoppingCart(shoppingCart: ShoppingCartDto): Observable<ShoppingCartCommandResponse> {
        // API call to update an existing shopping cart
        return this.http.post<ShoppingCartCommandResponse>(`${this.apiUrl}/update-user-shopcart`, shoppingCart);
    }

    resetShoppingCart(): Observable<BaseResponse> {
        return this.http.post<BaseResponse>(`${this.apiUrl}/reset-user-shopcart`, {});
    }

}