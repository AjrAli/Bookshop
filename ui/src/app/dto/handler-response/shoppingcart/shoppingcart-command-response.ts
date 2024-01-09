import { CommandResponse } from "../../response/command-response";
import { ShoppingCartResponseDto } from "../../shoppingcart/shoppingcart-response-dto";

export class ShoppingCartCommandResponse extends CommandResponse {

    shoppingCart: ShoppingCartResponseDto;

    constructor(message?: string, error?: boolean, validationErrors?: string[]) {
        super(message, error, validationErrors);
        this.shoppingCart = new ShoppingCartResponseDto();
    }
}