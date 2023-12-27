import { ShoppingCartResponseDto } from "../shoppingcart/shoppingcart-response-dto";

export class CustomerResponseDto {
    firstName: string = '';
    lastName: string = '';
    shippingAddressId: number = 0;
    billingAddressId: number = 0;
    shoppingCart: ShoppingCartResponseDto = new ShoppingCartResponseDto();
  
    constructor(
      firstName?: string,
      lastName?: string,
      shippingAddressId?: number,
      billingAddressId?: number,
      shoppingCart?: ShoppingCartResponseDto
    ) {
      // Initialize properties with default values if not provided
      this.firstName = firstName ?? this.firstName;
      this.lastName = lastName ?? this.lastName;
      this.shippingAddressId = shippingAddressId ?? this.shippingAddressId;
      this.billingAddressId = billingAddressId ?? this.billingAddressId;
      this.shoppingCart = shoppingCart ?? this.shoppingCart;
    }
}