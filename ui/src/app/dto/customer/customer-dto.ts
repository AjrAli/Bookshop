export class CustomerDto {
    username: string;
    password: string;
    confirmPassword: string;
    firstName: string;
    lastName: string;
    shippingAddressId: number
    shippingAddress: AddressDto
    billingAddressId: number
    billingAddress: AddressDto
    email: string;
    emailConfirmed: boolean;
    constructor() {
        this.username = '';
        this.password = '';
        this.confirmPassword = '';
        this.firstName = '';
        this.lastName = '';
        this.shippingAddressId = 0;
        this.shippingAddress = new AddressDto();
        this.billingAddressId = 0;
        this.billingAddress = new AddressDto();
        this.email = '';
        this.emailConfirmed = true;
    }
}
export class AddressDto {

    id: number
    street: string
    city: string
    postalCode: string
    country: string
    state: string

    constructor() {
        this.id = 0;
        this.street = '';
        this.city = '';
        this.postalCode = '';
        this.country = '';
        this.state = '';
    }
}