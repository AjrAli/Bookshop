import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CommonApiService } from "./common-api.service";

@Injectable()
export class ShoppingCartService extends CommonApiService {

    constructor(http: HttpClient) {
        super(http);
        this.apiUrl += '/shopcart';
    }
}