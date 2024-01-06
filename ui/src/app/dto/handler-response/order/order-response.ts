import { OrderResponseDto } from "../../order/order-response-dto";
import { BaseResponse } from "../../response/base-response";

export class OrderResponse extends BaseResponse {

    order: OrderResponseDto;

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.order = new OrderResponseDto();
    }
}