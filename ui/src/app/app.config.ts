import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
//import { provideAnimations } from '@angular/platform-browser/animations';
import {provideAnimationsAsync} from "@angular/platform-browser/animations/async";
import { routes } from './app.routes';
import { ProductService } from '../services/productservice';
import { PhotoService } from '../services/photoservice';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), ProductService, PhotoService, provideAnimationsAsync()]
};
