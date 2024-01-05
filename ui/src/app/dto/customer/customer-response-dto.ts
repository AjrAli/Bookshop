import { ShoppingCartResponseDto } from "../shoppingcart/shoppingcart-response-dto";

export class CustomerResponseDto {
  firstName: string = '';
  lastName: string = '';
  shippingAddress: string = '';
  billingAddress: string = '';
  shoppingCart: ShoppingCartResponseDto | undefined = undefined;

  constructor(data?: Partial<CustomerResponseDto>) {
    this.firstName = data?.firstName ?? this.firstName;
    this.lastName = data?.lastName ?? this.lastName;
    this.shippingAddress = data?.shippingAddress ?? this.shippingAddress;
    this.billingAddress = data?.billingAddress ?? this.billingAddress;
    this.shoppingCart = data?.shoppingCart ?? this.shoppingCart;
  }
}