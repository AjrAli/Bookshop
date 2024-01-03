import { BaseResponse } from "../../response/base-response";
import { ShoppingCartDetailsResponseDto } from "../../shoppingcart/shoppingcart-details-response-dto";

export class ShoppingCartDetailsResponse extends BaseResponse {

    shoppingCartDetails: ShoppingCartDetailsResponseDto;

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.shoppingCartDetails = new ShoppingCartDetailsResponseDto();
    }
}