import { AddressDto } from "./customer-dto";

export class EditProfileDto {
    firstName: string = '';
    lastName: string = '';
    shippingAddressId: number = 0;
    shippingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
    billingAddressId: number = 0;
    billingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  
    constructor(data?: Partial<EditProfileDto>) {
        // Initialize properties with default values if not provided
        this.firstName = data?.firstName ?? this.firstName;
        this.lastName = data?.lastName ?? this.lastName;
        this.shippingAddressId = data?.shippingAddressId ?? this.shippingAddressId;
        this.shippingAddress = new AddressDto(data?.shippingAddress) ?? this.shippingAddress;
        this.billingAddressId = data?.billingAddressId ?? this.billingAddressId;
        this.billingAddress = new AddressDto(data?.billingAddress) ?? this.billingAddress;
      }
}