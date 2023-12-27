import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
//import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { routes } from './app.routes';
import { ProductService } from '../services/productservice';
import { PhotoService } from '../services/photoservice';
import { JwtInterceptor } from '../services/jwt.interceptor';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ToastService } from '../services/toast.service';
import { BookService } from '../services/book.service';
import { ToastrModule } from 'ngx-toastr';
import { CustomerService } from '../services/customer.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), ProductService, PhotoService, provideAnimationsAsync(),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    importProvidersFrom(HttpClientModule),
    CustomerService,
    ToastService, BookService, importProvidersFrom(
      ToastrModule.forRoot()
    )
  ]
};
