import { ShoppingCartResponseDto } from "../shoppingcart/shoppingcart-response-dto";

export class CustomerResponseDto {
    firstName: string;
    lastName: string;
    shippingAddressId: number
    billingAddressId: number
    shoppingCart: ShoppingCartResponseDto;
    constructor() {
        this.firstName = '';
        this.lastName = '';
        this.shippingAddressId = 0;
        this.billingAddressId = 0;
        this.shoppingCart = new ShoppingCartResponseDto();
    }
}