import { ShoppingCartResponseDto } from "./shoppingcart-response-dto";

export class ShoppingCartDetailsResponseDto extends ShoppingCartResponseDto {

    subTotal: number;
    shippingFee: number;
    vatRate: number;

    constructor(data?: Partial<ShoppingCartDetailsResponseDto>) {
        super(data);
        this.subTotal = data?.subTotal ?? 0;
        this.shippingFee = data?.shippingFee ?? 0;
        this.vatRate = data?.vatRate ?? 0;
    }

}