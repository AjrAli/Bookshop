import { Injectable } from "@angular/core";
import { CustomerResponseDto } from "../../app/dto/customer/customer-response-dto";

@Injectable()
export class CustomerLocalStorageService {
    setCustomerInfo(customerResponseDto: CustomerResponseDto) {
        const serializedData = JSON.stringify(customerResponseDto);
        localStorage.setItem('customerData', serializedData);
    }

    getCustomerInfo(): CustomerResponseDto | null {
        const storedData = localStorage.getItem('customerData');
        if (storedData) {
            const deserializedCustomer = new CustomerResponseDto(JSON.parse(storedData));
            //console.log(deserializedCustomer);
            return deserializedCustomer;
        } else {
            console.log('No data found in localStorage');
        }
        return null;
    }

    removeCustomerDataStored() {
        localStorage.removeItem('customerData');
    }
}