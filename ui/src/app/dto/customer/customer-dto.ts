export class CustomerDto {
  username: string = '';
  password: string = '';
  confirmPassword: string = '';
  firstName: string = '';
  lastName: string = '';
  shippingAddressId: number = 0;
  shippingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  billingAddressId: number = 0;
  billingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  email: string = '';
  emailConfirmed: boolean = false;

  constructor(data?: Partial<CustomerDto>) {
    // Initialize properties with default values if not provided
    this.username = data?.username ?? this.username;
    this.password = data?.password ?? this.password;
    this.confirmPassword = data?.confirmPassword ?? this.confirmPassword;
    this.firstName = data?.firstName ?? this.firstName;
    this.lastName = data?.lastName ?? this.lastName;
    this.shippingAddressId = data?.shippingAddressId ?? this.shippingAddressId;
    this.shippingAddress = new AddressDto(data?.shippingAddress) ?? this.shippingAddress;
    this.billingAddressId = data?.billingAddressId ?? this.billingAddressId;
    this.billingAddress = new AddressDto(data?.billingAddress) ?? this.billingAddress;
    this.email = data?.email ?? this.email;
    this.emailConfirmed = data?.emailConfirmed ?? this.emailConfirmed;
  }
}
export class AddressDto {

  id: number = 0;
  street: string = '';
  city: string = '';
  postalCode: string = '';
  country: string = '';
  state: string = '';

  constructor(data?: Partial<AddressDto>) {
    // Initialize properties with default values if not provided
    this.id = data?.id ?? this.id;
    this.street = data?.street ?? this.street;
    this.city = data?.city ?? this.city;
    this.postalCode = data?.postalCode ?? this.postalCode;
    this.country = data?.country ?? this.country;
    this.state = data?.state ?? this.state;
  }
}