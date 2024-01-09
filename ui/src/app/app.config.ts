import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
//import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { routes } from './app.routes';
import { JwtInterceptor } from '../services/jwt.interceptor';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ToastService } from '../services/toast.service';
import { BookService } from '../services/book.service';
import { ToastrModule } from 'ngx-toastr';
import { CustomerService } from '../services/customer.service';
import { ShoppingCartService } from '../services/shoppingcart.service';
import { ShoppingCartApiService } from '../services/shoppingcart/shoppingcart-api.service';
import { ShoppingCartDataService } from '../services/shoppingcart/shoppingcart-data.service';
import { ShoppingCartLocalStorageService } from '../services/shoppingcart/shoppingcart-local-storage.service';
import { CustomerLocalStorageService } from '../services/customer/customer-local-storage.service';
import { CustomerApiService } from '../services/customer/customer-api.service';
import { TokenService } from '../services/token.service';
import { CustomerDataService } from '../services/customer/customer-data.service';
import { OrderApiService } from '../services/order/order-api.service';
import { OrderService } from '../services/order.service';
import { SearchService } from '../services/search/search.service';
import { SearchStateService } from '../services/search/search-state.service';
import { AuthorService } from '../services/author.service';
import { CategoryService } from '../services/category.service';
import { IdleTimeoutService } from '../services/idle-timeout.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), provideAnimationsAsync(),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    importProvidersFrom(HttpClientModule),
    TokenService,
    CustomerService,
    CustomerApiService,
    CustomerDataService,
    CustomerLocalStorageService,
    ShoppingCartService,
    ShoppingCartApiService,
    ShoppingCartLocalStorageService,
    ShoppingCartDataService,
    OrderApiService,
    OrderService,
    SearchService,
    SearchStateService,
    AuthorService,
    CategoryService,
    IdleTimeoutService,
    ToastService, BookService, importProvidersFrom(
      ToastrModule.forRoot()
    )
  ]
};