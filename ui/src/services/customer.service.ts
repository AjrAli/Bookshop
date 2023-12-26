import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthenticateResponse } from "../app/dto/handler-response/customer/authenticate/authenticate-response";
import { Role } from "../app/enum/role";
import { environment } from "../app/environments/environment";
import { CustomerDto } from "../app/dto/customer/customer-dto";
import { jwtDecode } from "jwt-decode";

@Injectable()
export class CustomerService {
  private readonly apiUrl = environment.apiUrl + '/customer';
  private userInfo: DecodedToken | undefined;

  constructor(private http: HttpClient) { }

  authenticate(username: string, password: string): Observable<AuthenticateResponse> {
    const body = { username, password };
    return this.http.post<AuthenticateResponse>(`${this.apiUrl}/authenticate`, body);
  }

  createCustomer(customer: CustomerDto): Observable<AuthenticateResponse> {
    return this.http.post<AuthenticateResponse>(`${this.apiUrl}/create-customer`, customer);
  }
  setToken(token: string) {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    this.userInfo = jwtDecode<DecodedToken>(token);
    const decodedToken: DecodedToken = jwtDecode(token);
    if (decodedToken.exp * 1000 < Date.now()) {
      // Token has expired
      this.logout();
      return false;
    }

    return true;
  }

  isAdmin(): boolean {
    if (this.isLoggedIn() && this.userInfo) {
      return this.userInfo.role === Role.Administrator;
    }

    return false;
  }

  logout() {
    localStorage.removeItem('authToken');
    this.userInfo = undefined;
  }
}

interface DecodedToken {
  role: Role;
  sub: string;
  jti: string;
  unique_name: string;
  exp: number;
  iss: string;
  aud: string;
}