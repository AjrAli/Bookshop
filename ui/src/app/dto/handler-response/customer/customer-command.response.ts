import { CustomerResponseDto } from "../../customer/customer-response-dto";
import { CommandResponse } from "../../response/command-response";

export class CustomerCommandResponse extends CommandResponse {
    customer: CustomerResponseDto;
    token: string;
    constructor(message?: string, error?: boolean, validationErrors?: string[]) {
        super(message, error, validationErrors);
        this.customer = new CustomerResponseDto();
        this.token = '';
    }
}