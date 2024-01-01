import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CustomerDto } from "../../app/dto/customer/customer-dto";
import { AuthenticateResponse } from "../../app/dto/handler-response/customer/authenticate/authenticate-response";
import { environment } from "../../app/environments/environment";

@Injectable()
export class CustomerApiService {
    private readonly apiUrl = environment.apiUrl + '/customer';

    constructor(private http: HttpClient) { }

    authenticate(username: string, password: string) {
        const body = { username, password };
        return this.http.post<AuthenticateResponse>(`${this.apiUrl}/authenticate`, body);
    }

    createCustomer(customer: CustomerDto) {
        return this.http.post<AuthenticateResponse>(`${this.apiUrl}/create-customer`, customer);
    }
}