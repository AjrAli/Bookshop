import { CustomerResponseDto } from "../../../customer/customer-response-dto";
import { BaseResponse } from "../../../response/base-response";

export class AuthenticateResponse extends BaseResponse {

    customer: CustomerResponseDto;
    token: string;

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.customer = new CustomerResponseDto();
        this.token = '';
    }
}