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
    protected apiUrl: string = environment.apiUrl + '/shopcarts';

    constructor(private http: HttpClient) { }

    getShoppingCart(): Observable<ShoppingCartResponse> {
        // API call to get a full details of shopping cart
        return this.http.get<ShoppingCartResponse>(`${this.apiUrl}/current`);
    }

    getShoppingCartDetails(): Observable<ShoppingCartDetailsResponse> {
        // API call to get a full details of shopping cart
        return this.http.get<ShoppingCartDetailsResponse>(`${this.apiUrl}/current/reviews`);
    }

    createShoppingCart(shoppingCart: ShoppingCartDto): Observable<ShoppingCartCommandResponse> {
        // API call to create a new shopping cart
        return this.http.post<ShoppingCartCommandResponse>(`${this.apiUrl}`, shoppingCart);
    }

    updateShoppingCart(shoppingCart: ShoppingCartDto): Observable<ShoppingCartCommandResponse> {
        // API call to update an existing shopping cart
        return this.http.put<ShoppingCartCommandResponse>(`${this.apiUrl}`, shoppingCart);
    }

    resetShoppingCart(): Observable<BaseResponse> {
        return this.http.put<BaseResponse>(`${this.apiUrl}/reset`, {});
    }

}