import { OrderResponseDto } from "../../order/order-response-dto";
import { BaseResponse } from "../../response/base-response";

export class OrdersResponse extends BaseResponse {

    orders: OrderResponseDto[];

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.orders = [];
    }
}