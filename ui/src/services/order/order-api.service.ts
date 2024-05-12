import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseResponse } from "../../app/dto/response/base-response";
import { OrderDto } from "../../app/dto/order/order-dto";
import { UpdateOrderDto } from "../../app/dto/order/update-order-dto";
import { OrdersResponse } from "../../app/dto/handler-response/order/orders-response";
import { OrderCommandResponse } from "../../app/dto/handler-response/order/order-command.response";
import { environment } from "../../app/environments/environment";
import { OrderResponse } from "../../app/dto/handler-response/order/order-response";

@Injectable()
export class OrderApiService {
    protected apiUrl: string = environment.apiUrl + '/orders';

    constructor(private http: HttpClient) { }

    getOrders(): Observable<OrdersResponse> {
        return this.http.get<OrdersResponse>(`${this.apiUrl}`);
    }
    getOrderById(id: number): Observable<OrderResponse> {
        return this.http.get<OrderResponse>(`${this.apiUrl}/${id}`);
    }
    createOrder(order: OrderDto): Observable<OrderCommandResponse> {
        return this.http.post<OrderCommandResponse>(`${this.apiUrl}`, order);
    }

    cancelOrder(id: number): Observable<BaseResponse> {
        return this.http.put<BaseResponse>(`${this.apiUrl}/${id}/cancel`, {});
    }

    updateOrder(updateOrder: UpdateOrderDto, id:number): Observable<OrderCommandResponse> {
        return this.http.put<OrderCommandResponse>(`${this.apiUrl}/${id}`, updateOrder);
    }

}