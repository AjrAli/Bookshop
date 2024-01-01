import { ShoppingCartResponseDto } from "../shoppingcart/shoppingcart-response-dto";

export class CustomerResponseDto {
  firstName: string = '';
  lastName: string = '';
  shippingAddressId: number = 0;
  billingAddressId: number = 0;
  shoppingCart: ShoppingCartResponseDto | undefined = undefined;

  constructor(data?: Partial<CustomerResponseDto>) {
    this.firstName = data?.firstName ?? this.firstName;
    this.lastName = data?.lastName ?? this.lastName;
    this.shippingAddressId = data?.shippingAddressId ?? this.shippingAddressId;
    this.billingAddressId = data?.billingAddressId ?? this.billingAddressId;
    this.shoppingCart = data?.shoppingCart ?? this.shoppingCart;
  }
}