import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseResponse } from "../../app/dto/response/base-response";
import { CommonApiService } from "../common-api.service";
import { OrderDto } from "../../app/dto/order/order-dto";
import { OrderResponse } from "../../app/dto/handler-response/order/order-response";
import { UpdateOrderDto } from "../../app/dto/order/update-order-dto";
import { OrdersResponse } from "../../app/dto/handler-response/order/orders-response";

@Injectable()
export class OrderApiService extends CommonApiService {
    constructor(http: HttpClient) {
        super(http);
        this.apiUrl += '/order';
    }

    getOrders(): Observable<OrdersResponse> {
        return this.http.get<OrdersResponse>(`${this.apiUrl}/get-user-orders`);
    }
    getOrderById(id: number): Observable<OrdersResponse> {
        return this.http.get<OrdersResponse>(`${this.apiUrl}/get-user-order/${id}`);
    }
    createOrder(order: OrderDto): Observable<OrderResponse> {
        return this.http.post<OrderResponse>(`${this.apiUrl}/create-user-order`, order);
    }

    cancelOrder(id: number): Observable<BaseResponse> {
        return this.http.post<BaseResponse>(`${this.apiUrl}/cancel-user-order`, id);
    }

    updateOrder(updateOrder: UpdateOrderDto): Observable<OrderResponse> {
        return this.http.post<OrderResponse>(`${this.apiUrl}/update-user-order`, updateOrder);
    }

}