import { Injectable } from "@angular/core";
import { AuthenticateResponse } from "../app/dto/handler-response/customer/authenticate/authenticate-response";
import { Role } from "../app/enum/role";
import { CustomerDto } from "../app/dto/customer/customer-dto";
import { CustomerResponseDto } from "../app/dto/customer/customer-response-dto";
import { ToastService } from "./toast.service";
import { ShoppingCartResponseDto } from "../app/dto/shoppingcart/shoppingcart-response-dto";
import { ShoppingCartDto } from "../app/dto/shoppingcart/shoppingcart-dto";
import { ShoppingCartService } from "./shoppingcart.service";
import { ShoppingCartDataService } from "./shoppingcart/shoppingcart-data.service";
import { CustomerApiService } from "./customer/customer-api.service";
import { CustomerLocalStorageService } from "./customer/customer-local-storage.service";
import { DecodedToken, TokenService } from "./token.service";
import { BehaviorSubject, Observable, catchError, map, of, switchMap, tap } from "rxjs";
import { CustomerDataService } from "./customer/customer-data.service";
import { CustomerCommandResponse } from "../app/dto/handler-response/customer/customer-command.response";
import { EditProfileDto } from "../app/dto/customer/edit-profile-dto";
import { EditPasswordDto } from "../app/dto/customer/edit-password-dto";

@Injectable()
export class CustomerService {
  private userInfo: DecodedToken | undefined;

  constructor(private customerApiService: CustomerApiService,
    private customerLocalStorageService: CustomerLocalStorageService,
    private customerDataService: CustomerDataService,
    private toastService: ToastService,
    private shoppingCartService: ShoppingCartService,
    private shoppingCartDataService: ShoppingCartDataService,
    private tokenService: TokenService) {
    this.loadCustomer();
  }
  private loadCustomer() {
    const customer = this.customerLocalStorageService.getCustomerInfo();
    if (customer) {
      this.customerDataService.setCustomer(customer);
      this.setCustomerShoppingCart(customer);
    }
  }

  authenticate(username: string, password: string): Observable<boolean> {
    return this.customerApiService.authenticate(username, password).pipe(tap({
      next: (r) => this.handleAuthenticationResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    }), map(r => !!r.token)
    );
  }

  createCustomer(customer: CustomerDto): Observable<boolean> {
    return this.customerApiService.createCustomer(customer).pipe(tap({
      next: (r) => this.handleCustomerCommandResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    }), map(r => !!r.token)
    );
  }
  editProfile(editProfile: EditProfileDto): Observable<boolean> {
    return this.customerApiService.editProfile(editProfile).pipe(tap({
      next: (r) => this.handleCustomerCommandResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    }), map(r => !!r.token)
    );
  }
  editPassword(editPassword: EditPasswordDto): Observable<boolean> {
    return this.customerApiService.editPassword(editPassword).pipe(tap({
      next: (r) => this.handleCustomerCommandResponse(r),
      error: (e) => this.handleAuthenticationError(e),
      complete: () => console.info('complete')
    }), map(r => !!r.token)
    );
  }
  private handleCustomerCommandResponse(response: CustomerCommandResponse): void {
    if (response.token) {
      this.tokenService.setToken(response.token);
      if (response.customer) {
        this.setCustomerShoppingCart(response.customer);
        this.customerDataService.setCustomer(response.customer);
        this.customerLocalStorageService.setCustomerInfo(response.customer);
      }
      this.toastService.showSuccess(response.message);
    } else {
      this.toastService.showValidationError(response);
    }
  }
  private handleAuthenticationResponse(response: AuthenticateResponse): void {
    if (response.token) {
      this.tokenService.setToken(response.token);
      if (response.customer) {
        this.setCustomerShoppingCart(response.customer);
        this.customerDataService.setCustomer(response.customer);
        this.customerLocalStorageService.setCustomerInfo(response.customer);
      }
      this.toastService.showSuccess(response.message);
    } else {
      this.toastService.showSimpleError('Invalid credentials');
    }
  }

  private handleAuthenticationError(error: any): void {
    this.toastService.showError(error.error);
    this.toastService.showError(error);
  }

  private setCustomerShoppingCart(customerResponseDto: CustomerResponseDto) {
    if (customerResponseDto.shoppingCart) {
      customerResponseDto.shoppingCart = new ShoppingCartResponseDto(customerResponseDto.shoppingCart);
      this.shoppingCartService.updateFullyShoppingCart(customerResponseDto.shoppingCart)
    }
  }

  isLoggedIn(): boolean {
    const token = this.tokenService.getToken();
    if (!token) {
      this.resetFullyCustomer();
      return false;
    }
    this.userInfo = this.tokenService.decodeToken(token);
    if (this.tokenService.isTokenExpired(token)) {
      this.resetFullyCustomer();
      return false;
    }
    if (!this.customerDataService.getCustomer())
      this.loadCustomer();
    return true;
  }

  isAdmin(): boolean {
    if (this.isLoggedIn() && this.userInfo) {
      return this.userInfo.role === Role.Administrator;
    }
    return false;
  }
  updateCustomerShoppingCart(): Observable<boolean> {
    /**
     *     Use switchMap when you need to switch to a new observable based on the values emitted by the source observable.
           Use tap when you want to perform side effects without modifying the observable's flow.
     */
    return this.syncShoppingCartWithCustomer()?.pipe(switchMap(
      (r) => {
        // Updated shoppingcart by API
        if (r && typeof r !== 'boolean') {
          const shoppingCart = r as ShoppingCartResponseDto;
          const customer = this.customerDataService.getCustomer();
          if (customer && shoppingCart) {
            customer.shoppingCart = shoppingCart
            this.setCustomerShoppingCart(customer);
            this.customerDataService.setCustomer(customer);
            this.customerLocalStorageService.setCustomerInfo(customer);
            return of(true);
          }
        }
        // Case no change because same shoppingcart detected
        if (r === true)
          return of(true);
        else
          return of(false);
      }),
      catchError(() => of(false))
    ) || of(false);
  }

  syncShoppingCartWithCustomer(): Observable<ShoppingCartResponseDto | boolean> {
    const shoppingCartResponseDto = this.shoppingCartDataService.getShoppingCart();
    if (shoppingCartResponseDto) {
      const shoppingCart = new ShoppingCartDto(shoppingCartResponseDto)
      const getCustomerPreviousShoppingCart = this.customerDataService.getCustomer()?.shoppingCart;
      const isShoppingCartOfCustomerNotDifferent = shoppingCartResponseDto.equals(getCustomerPreviousShoppingCart);
      if (getCustomerPreviousShoppingCart && !isShoppingCartOfCustomerNotDifferent) {
        if (shoppingCart.items.length > 0)
          return this.shoppingCartService.updateShoppingCartFromApi(shoppingCart);
        else
          return this.shoppingCartService.resetShoppingCartFromApi();
      } else {
        if (!isShoppingCartOfCustomerNotDifferent && shoppingCart.items.length > 0)
          return this.shoppingCartService.createShoppingCartFromApi(shoppingCart);
      }
    }
    return of(true);
  }
  resetFullyCustomer() {
    this.customerDataService.resetCustomer();
    this.customerLocalStorageService.removeCustomerDataStored();
    this.tokenService.removeTokenStored();
  }
  resetFullyLocalShoppingCart() {
    this.shoppingCartService.resetLocalShoppingCart();
    const customer = this.customerDataService.getCustomer();
    if (customer) {
      customer.shoppingCart = new ShoppingCartResponseDto();
      this.customerDataService.setCustomer(customer);
      this.customerLocalStorageService.setCustomerInfo(customer);
    }
  }
  logout() {
    this.syncShoppingCartWithCustomer()?.subscribe({
      next: (r) => {
        if (r) {
          this.shoppingCartService.resetLocalShoppingCart();
          this.resetFullyCustomer();
          this.userInfo = undefined;
        }
      }
    })
  }
}

