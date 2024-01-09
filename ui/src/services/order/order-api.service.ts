import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseResponse } from "../../app/dto/response/base-response";
import { OrderDto } from "../../app/dto/order/order-dto";
import { UpdateOrderDto } from "../../app/dto/order/update-order-dto";
import { OrdersResponse } from "../../app/dto/handler-response/order/orders-response";
import { OrderCommandResponse } from "../../app/dto/handler-response/order/order-command.response";
import { environment } from "../../app/environments/environment";

@Injectable()
export class OrderApiService {
    protected apiUrl: string = environment.apiUrl + '/order';

    constructor(private http: HttpClient) { }

    getOrders(): Observable<OrdersResponse> {
        return this.http.get<OrdersResponse>(`${this.apiUrl}/get-user-orders`);
    }
    getOrderById(id: number): Observable<OrdersResponse> {
        return this.http.get<OrdersResponse>(`${this.apiUrl}/get-user-order/${id}`);
    }
    createOrder(order: OrderDto): Observable<OrderCommandResponse> {
        return this.http.post<OrderCommandResponse>(`${this.apiUrl}/create-user-order`, order);
    }

    cancelOrder(id: number): Observable<BaseResponse> {
        return this.http.post<BaseResponse>(`${this.apiUrl}/cancel-user-order`, id);
    }

    updateOrder(updateOrder: UpdateOrderDto): Observable<OrderCommandResponse> {
        return this.http.post<OrderCommandResponse>(`${this.apiUrl}/update-user-order`, updateOrder);
    }

}