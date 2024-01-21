import { Injectable } from "@angular/core";
import { OrderDto } from "../app/dto/order/order-dto";
import { OrderResponseDto } from "../app/dto/order/order-response-dto";
import { OrderApiService } from "./order/order-api.service";
import { ToastService } from "./toast.service";
import { Observable, map, tap } from "rxjs";
import { UpdateOrderDto } from "../app/dto/order/update-order-dto";
import { OrderCommandResponse } from "../app/dto/handler-response/order/order-command.response";

@Injectable()
export class OrderService {


    constructor(private orderApiService: OrderApiService,
        private toastService: ToastService,
    ) { }

    createOrderFromApi(order: OrderDto): Observable<OrderResponseDto> {
        return this.orderApiService.createOrder(order).pipe(tap({
            next: (r) => this.handleOrderResponse(r),
            error: (e) => this.handleOrderError(e)
        }), map(response => response.order));
    }
    updateOrderFromApi(updateOrder: UpdateOrderDto): Observable<OrderResponseDto> {
        return this.orderApiService.updateOrder(updateOrder).pipe(tap({
            next: (r) => this.handleOrderResponse(r),
            error: (e) => this.handleOrderError(e)
        }), map(response => response.order));
    }
    cancelOrderFromApi(id: number): Observable<boolean> {
        return this.orderApiService.cancelOrder(id).pipe(tap({
            next: (r) => this.toastService.showSuccess(r.message),
            error: (e) => this.handleOrderError(e)
        }), map(response => response.success));
    }

    private handleOrderResponse(response: OrderCommandResponse): void {
        if (response.order) {
            this.toastService.showSuccess(response.message);
        } else {
            this.toastService.showValidationError(response);
        }
    }

    private handleOrderError(error: any): void {
        this.toastService.showError(error.error);
        this.toastService.showError(error);
    }
}