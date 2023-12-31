import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, Observer } from "rxjs";
import { AuthenticateResponse } from "../app/dto/handler-response/customer/authenticate/authenticate-response";
import { Role } from "../app/enum/role";
import { environment } from "../app/environments/environment";
import { CustomerDto } from "../app/dto/customer/customer-dto";
import { jwtDecode } from "jwt-decode";
import { CustomerResponseDto } from "../app/dto/customer/customer-response-dto";
import { ActivatedRoute, Router } from "@angular/router";
import { ValidationErrorResponse } from "../app/dto/response/error/validation-error-response";
import { ToastService } from "./toast.service";
import { ShoppingCartService } from "./shoppingcart.service";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { ShoppingCartDto } from "../app/dto/shoppingcart/shoppingcart-dto";

@Injectable()
export class CustomerService {
  private readonly apiUrl = environment.apiUrl + '/customer';
  private userInfo: DecodedToken | undefined;

  constructor(private http: HttpClient,
    private route: ActivatedRoute,
    private toastService: ToastService,
    private router: Router,
    private shoppingCartService: ShoppingCartService) { }

  authenticate(username: string, password: string) {
    const body = { username, password };
    this.http.post<AuthenticateResponse>(`${this.apiUrl}/authenticate`, body).subscribe({
      next: (r) => this.handleAuthenticationResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    });
  }

  createCustomer(customer: CustomerDto) {
    this.http.post<AuthenticateResponse>(`${this.apiUrl}/create-customer`, customer).subscribe({
      next: (r) => this.handleAuthenticationResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    });
  }

  private handleAuthenticationResponse(response: AuthenticateResponse): void {
    if (response.token) {
      this.setToken(response.token);
      if (response.customer) this.setCustomerInfo(response.customer);
      const returnUrl = this.route.snapshot.queryParams['returnUrl'];
      this.toastService.showSuccess(response.message);
      this.router.navigateByUrl(returnUrl ?? '');
    } else {
      this.toastService.showSimpleError('Invalid credentials');
    }
  }

  private handleAuthenticationError(error: any): void {
    this.toastService.showError(error.error as ValidationErrorResponse);
    this.toastService.showError(error as ValidationErrorResponse);
  }

  setCustomerInfo(customerResponseDto: CustomerResponseDto) {
    if (customerResponseDto.shoppingCart) {
      customerResponseDto.shoppingCart = new ShoppingCartResponseDto(customerResponseDto.shoppingCart);
      this.shoppingCartService.updateShoppingCart(customerResponseDto.shoppingCart);
    }
    const serializedData = JSON.stringify(customerResponseDto);
    sessionStorage.setItem('customerData', serializedData);
  }

  getCustomerInfo(): CustomerResponseDto | null {
    const storedData = sessionStorage.getItem('customerData');
    if (storedData) {
      const deserializedCustomer = new CustomerResponseDto(JSON.parse(storedData));
      //console.log(deserializedCustomer);
      return deserializedCustomer;
    } else {
      console.log('No data found in sessionStorage');
    }
    return null;
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
    const shoppingCartResponseDto = this.shoppingCartService.getShoppingCart();
    if (shoppingCartResponseDto && shoppingCartResponseDto.items.length > 0) {
      const shoppingCart = new ShoppingCartDto(shoppingCartResponseDto)
      if (this.getCustomerInfo()?.shoppingCart) {
        this.shoppingCartService.updateShoppingCartToApi(shoppingCart);
      } else {
        this.shoppingCartService.createShoppingCartToApi(shoppingCart);
      }
      this.shoppingCartService.resetShoppingCart();
    }
    localStorage.removeItem('authToken');
    sessionStorage.removeItem('customerData');
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