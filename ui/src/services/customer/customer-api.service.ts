import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CustomerDto } from "../../app/dto/customer/customer-dto";
import { AuthenticateResponse } from "../../app/dto/handler-response/customer/authenticate/authenticate-response";
import { environment } from "../../app/environments/environment";
import { EditProfileDto } from "../../app/dto/customer/edit-profile-dto";
import { CustomerCommandResponse } from "../../app/dto/handler-response/customer/customer-command.response";
import { EditPasswordDto } from "../../app/dto/customer/edit-password-dto";

@Injectable()
export class CustomerApiService {
    private readonly apiUrl = environment.apiUrl + '/customers';

    constructor(private http: HttpClient) { }

    authenticate(username: string, password: string) {
        const body = { username, password };
        return this.http.post<AuthenticateResponse>(`${this.apiUrl}/authenticate`, body);
    }

    createCustomer(customer: CustomerDto) {
        return this.http.post<CustomerCommandResponse>(`${this.apiUrl}`, customer);
    }
    editProfile(editProfile: EditProfileDto) {
        return this.http.put<CustomerCommandResponse>(`${this.apiUrl}`, editProfile);
    }
    editPassword(editPassword: EditPasswordDto) {
        return this.http.put<CustomerCommandResponse>(`${this.apiUrl}/password`, editPassword);
    }
}