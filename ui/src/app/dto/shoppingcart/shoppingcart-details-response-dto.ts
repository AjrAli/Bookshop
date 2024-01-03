import { ShoppingCartResponseDto } from "./shoppingcart-response-dto";

export class ShoppingCartDetailsResponseDto extends ShoppingCartResponseDto {

    id: number;
    subTotal: number;
    shippingFee: number;
    vatRate: number;

    constructor(data?: Partial<ShoppingCartDetailsResponseDto>) {
        super(data);
        this.id = data?.id ?? 0;
        this.subTotal = data?.subTotal ?? 0;
        this.shippingFee = data?.shippingFee ?? 0;
        this.vatRate = data?.vatRate ?? 0;
    }

}