import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { ProductService } from '../services/productservice';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), ProductService]
};
