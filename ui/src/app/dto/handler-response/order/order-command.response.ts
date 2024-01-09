import { OrderResponseDto } from "../../order/order-response-dto";
import { CommandResponse } from "../../response/command-response";

export class OrderCommandResponse extends CommandResponse {
    order: OrderResponseDto;
    constructor(message?: string, error?: boolean, validationErrors?: string[]) {
        super(message, error, validationErrors);
        this.order = new OrderResponseDto();
    }
}