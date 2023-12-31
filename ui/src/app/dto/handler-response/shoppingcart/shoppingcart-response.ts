import { BaseResponse } from "../../response/base-response";
import { ShoppingCartResponseDto } from "../../shoppingcart/shoppingcart-response-dto";

export class ShoppingCartResponse extends BaseResponse {

    shoppingCart: ShoppingCartResponseDto;

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.shoppingCart = new ShoppingCartResponseDto();
    }
}