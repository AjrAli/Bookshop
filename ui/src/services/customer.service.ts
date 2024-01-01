import { Injectable } from "@angular/core";
import { AuthenticateResponse } from "../app/dto/handler-response/customer/authenticate/authenticate-response";
import { Role } from "../app/enum/role";
import { CustomerDto } from "../app/dto/customer/customer-dto";
import { CustomerResponseDto } from "../app/dto/customer/customer-response-dto";
import { ActivatedRoute, Router } from "@angular/router";
import { ValidationErrorResponse } from "../app/dto/response/error/validation-error-response";
import { ToastService } from "./toast.service";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { ShoppingCartDto } from "../app/dto/shoppingcart/shoppingcart-dto";
import { ShoppingCartService } from "./shoppingcart.service";
import { ShoppingCartDataService } from "./shoppingcart/shoppingcart-data.service";
import { CustomerApiService } from "./customer/customer-api.service";
import { CustomerLocalStorageService } from "./customer/customer-local-storage.service";
import { DecodedToken, TokenService } from "./token.service";

@Injectable()
export class CustomerService {
  private userInfo: DecodedToken  | undefined;

  constructor(private customerApiService: CustomerApiService,
    private customerLocalStorageService: CustomerLocalStorageService,
    private route: ActivatedRoute,
    private toastService: ToastService,
    private router: Router,
    private shoppingCartService: ShoppingCartService,
    private shoppingCartDataService: ShoppingCartDataService,
    private tokenService: TokenService) { }

  authenticate(username: string, password: string) {
    this.customerApiService.authenticate(username, password).subscribe({
      next: (r) => this.handleAuthenticationResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    });
  }

  createCustomer(customer: CustomerDto) {
    this.customerApiService.createCustomer(customer).subscribe({
      next: (r) => this.handleAuthenticationResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    });
  }

  private handleAuthenticationResponse(response: AuthenticateResponse): void {
    if (response.token) {
      this.customerLocalStorageService.setToken(response.token);
      if (response.customer) {
        this.setCustomerShoppingCart(response.customer);
        this.customerLocalStorageService.setCustomerInfo(response.customer);
      }
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

  private setCustomerShoppingCart(customerResponseDto: CustomerResponseDto) {
    if (customerResponseDto.shoppingCart) {
      customerResponseDto.shoppingCart = new ShoppingCartResponseDto(customerResponseDto.shoppingCart);
      this.shoppingCartService.incrementallyUpdateShoppingCart(customerResponseDto.shoppingCart);
    }
  }

  isLoggedIn(): boolean {
    const token = this.customerLocalStorageService.getToken();
    if (!token) {
      return false;
    }
    this.userInfo = this.tokenService.decodeToken(token);
    if (this.tokenService.isTokenExpired(token)) {
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
    const shoppingCartResponseDto = this.shoppingCartDataService.getShoppingCart();
    if (shoppingCartResponseDto && shoppingCartResponseDto.items.length > 0) {
      const shoppingCart = new ShoppingCartDto(shoppingCartResponseDto)
      const getCustomerPreviousShoppingCart = this.customerLocalStorageService.getCustomerInfo()?.shoppingCart;
     
      if (getCustomerPreviousShoppingCart && getCustomerPreviousShoppingCart.items?.length > 0) {
        this.shoppingCartService.updateShoppingCartToApi(shoppingCart);
      } else {
        this.shoppingCartService.createShoppingCartToApi(shoppingCart);
      }
      this.shoppingCartService.resetFullyShoppingCart();
    }
    this.customerLocalStorageService.removeCustomerDataStored();
    this.userInfo = undefined;
  }
}
