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

  constructor(
    private customerApiService: CustomerApiService,
    private customerLocalStorageService: CustomerLocalStorageService,
    private customerDataService: CustomerDataService,
    private toastService: ToastService,
    private shoppingCartService: ShoppingCartService,
    private shoppingCartDataService: ShoppingCartDataService,
    private tokenService: TokenService
  ) {
    this.loadCustomer();
  }

  // Load customer information from local storage
  private loadCustomer() {
    const customer = this.customerLocalStorageService.getCustomerInfo();
    if (customer) {
      this.customerDataService.setCustomer(customer);
      this.setCustomerShoppingCart(customer);
    }
  }

  // Authenticate user with provided credentials
  authenticate(username: string, password: string): Observable<boolean> {
    return this.customerApiService.authenticate(username, password).pipe(
      tap({
        next: (response) => this.handleAuthenticationResponse(response),
        error: (error) => this.handleAuthenticationError(error),
        complete: () => console.info('Authentication complete')
      }),
      map(response => !!response.token)
    );
  }

  // Create a new customer
  createCustomer(customer: CustomerDto): Observable<boolean> {
    return this.customerApiService.createCustomer(customer).pipe(
      tap({
        next: (response) => this.handleCustomerCommandResponse(response),
        error: (error) => this.handleAuthenticationError(error),
        complete: () => console.info('Customer creation complete')
      }),
      map(response => !!response.token)
    );
  }

  // Edit customer profile
  editProfile(editProfile: EditProfileDto): Observable<boolean> {
    return this.customerApiService.editProfile(editProfile).pipe(
      tap({
        next: (response) => this.handleCustomerCommandResponse(response),
        error: (error) => this.handleAuthenticationError(error),
        complete: () => console.info('Profile editing complete')
      }),
      map(response => !!response.token)
    );
  }

  // Edit customer password
  editPassword(editPassword: EditPasswordDto): Observable<boolean> {
    return this.customerApiService.editPassword(editPassword).pipe(
      tap({
        next: (response) => this.handleCustomerCommandResponse(response),
        error: (error) => this.handleAuthenticationError(error),
        complete: () => console.info('Password editing complete')
      }),
      map(response => !!response.token)
    );
  }

  // Handle response from customer command (create, edit profile, edit password)
  private handleCustomerCommandResponse(response: CustomerCommandResponse): void {
    if (response.token) {
      this.tokenService.setToken(response.token);
      if (response.customer) {
        this.setAllCustomerData(response.customer);
      }
      this.toastService.showSuccess(response.message);
    } else {
      this.toastService.showValidationError(response);
    }
  }

  // Handle authentication response
  private handleAuthenticationResponse(response: AuthenticateResponse): void {
    if (response.token) {
      this.tokenService.setToken(response.token);
      if (response.customer) {
        this.setAllCustomerData(response.customer);
      }
      this.toastService.showSuccess(response.message);
    } else {
      this.toastService.showSimpleError('Invalid credentials');
    }
  }

  // Handle authentication error
  private handleAuthenticationError(error: any): void {
    this.toastService.showError(error.error);
    this.toastService.showError(error);
  }

  // Set customer shopping cart details
  private setCustomerShoppingCart(customerResponseDto: CustomerResponseDto) {
    if (customerResponseDto.shoppingCart) {
      customerResponseDto.shoppingCart = new ShoppingCartResponseDto(customerResponseDto.shoppingCart);
      this.shoppingCartService.updateFullyShoppingCart(customerResponseDto.shoppingCart);
    }
  }

  // Check if the user is logged in
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
    if (!this.customerDataService.getCustomer()) {
      this.loadCustomer();
    }
    return true;
  }

  // Check if the logged-in user is an administrator
  isAdmin(): boolean {
    if (this.isLoggedIn() && this.userInfo) {
      return this.userInfo.role === Role.Administrator;
    }
    return false;
  }

  // Update customer shopping cart from the server
  updateCustomerShoppingCart(): Observable<boolean> {
    return (
      this.syncShoppingCartWithCustomer()?.pipe(
        switchMap((response) => {
          // Updated shopping cart by API
          if (response && typeof response !== 'boolean') {
            const shoppingCart = response as ShoppingCartResponseDto;
            const customer = this.customerDataService.getCustomer();
            if (customer && shoppingCart) {
              customer.shoppingCart = shoppingCart;
              this.setAllCustomerData(customer);
              return of(true);
            }
          }
          // Case no change because the same shopping cart detected
          if (response === true) {
            return of(true);
          } else {
            return of(false);
          }
        }),
        catchError(() => of(false))
      ) || of(false)
    );
  }

  // Sync customer shopping cart with the server
  syncShoppingCartWithCustomer(): Observable<ShoppingCartResponseDto | boolean> {
    const shoppingCartResponseDto = this.shoppingCartDataService.getShoppingCart();
    if (shoppingCartResponseDto) {
      const shoppingCart = new ShoppingCartDto(shoppingCartResponseDto);
      const getCustomerPreviousShoppingCart = this.customerDataService.getCustomer()?.shoppingCart;
      const isShoppingCartOfCustomerNotDifferent = shoppingCartResponseDto.equals(getCustomerPreviousShoppingCart);
      if (getCustomerPreviousShoppingCart && !isShoppingCartOfCustomerNotDifferent) {
        if (shoppingCart.items.length > 0) {
          return this.shoppingCartService.updateShoppingCartFromApi(shoppingCart);
        } else {
          return this.shoppingCartService.resetShoppingCartFromApi();
        }
      } else {
        if (!isShoppingCartOfCustomerNotDifferent && shoppingCart.items.length > 0) {
          return this.shoppingCartService.createShoppingCartFromApi(shoppingCart);
        }
      }
    }
    return of(true);
  }

  // Reset all customer-related data
  resetFullyCustomer() {
    this.customerDataService.resetCustomer();
    this.customerLocalStorageService.removeCustomerDataStored();
    this.tokenService.removeTokenStored();
  }

  // Reset local shopping cart data
  resetFullyLocalShoppingCart() {
    this.shoppingCartService.resetLocalShoppingCart();
    const customer = this.customerDataService.getCustomer();
    if (customer) {
      customer.shoppingCart = new ShoppingCartResponseDto();
      this.setAllCustomerData(customer);
    }
  }
  setAllCustomerData(customer: CustomerResponseDto) {
    this.setCustomerShoppingCart(customer);
    this.customerDataService.setCustomer(customer);
    this.customerLocalStorageService.setCustomerInfo(customer);
  }
  // Logout the user
  logout() {
    this.syncShoppingCartWithCustomer()?.subscribe({
      next: (response) => {
        if (response) {
          this.shoppingCartService.resetLocalShoppingCart();
          this.resetFullyCustomer();
          this.userInfo = undefined;
        }
      }
    });
  }
}
