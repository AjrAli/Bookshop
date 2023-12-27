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
  
    constructor(
      username?: string,
      password?: string,
      confirmPassword?: string,
      firstName?: string,
      lastName?: string,
      shippingAddressId?: number,
      shippingAddress?: AddressDto,
      billingAddressId?: number,
      billingAddress?: AddressDto,
      email?: string,
      emailConfirmed?: boolean
    ) {
      // Initialize properties with default values if not provided
      this.username = username ?? this.username;
      this.password = password ?? this.password;
      this.confirmPassword = confirmPassword ?? this.confirmPassword;
      this.firstName = firstName ?? this.firstName;
      this.lastName = lastName ?? this.lastName;
      this.shippingAddressId = shippingAddressId ?? this.shippingAddressId;
      this.shippingAddress = shippingAddress ?? this.shippingAddress;
      this.billingAddressId = billingAddressId ?? this.billingAddressId;
      this.billingAddress = billingAddress ?? this.billingAddress;
      this.email = email ?? this.email;
      this.emailConfirmed = emailConfirmed ?? this.emailConfirmed;
    }
}
export class AddressDto {

    id: number = 0;
    street: string = '';
    city: string = '';
    postalCode: string = '';
    country: string = '';
    state: string = '';
  
    constructor(
      id?: number,
      street?: string,
      city?: string,
      postalCode?: string,
      country?: string,
      state?: string
    ) {
      // Initialize properties with default values if not provided
      this.id = id ?? this.id;
      this.street = street ?? this.street;
      this.city = city ?? this.city;
      this.postalCode = postalCode ?? this.postalCode;
      this.country = country ?? this.country;
      this.state = state ?? this.state;
    }
}