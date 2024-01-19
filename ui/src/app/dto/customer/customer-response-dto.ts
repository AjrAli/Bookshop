import { ShoppingCartResponseDto } from "../shoppingcart/shoppingcart-response-dto";

export class CustomerResponseDto {
  userName: string = '';
  email: string = '';
  firstName: string = '';
  lastName: string = '';
  shippingAddress: string = '';
  billingAddress: string = '';
  shoppingCart: ShoppingCartResponseDto | undefined = undefined;

  constructor(data?: Partial<CustomerResponseDto>) {
    this.userName = data?.userName ?? this.userName;
    this.email = data?.email ?? this.email;
    this.firstName = data?.firstName ?? this.firstName;
    this.lastName = data?.lastName ?? this.lastName;
    this.shippingAddress = data?.shippingAddress ?? this.shippingAddress;
    this.billingAddress = data?.billingAddress ?? this.billingAddress;
    this.shoppingCart = new ShoppingCartResponseDto(data?.shoppingCart) ?? this.shoppingCart;
  }
}