import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';
import { LoginFormComponent } from '../../../forms/login-form/login-form.component';
import { SignUpFormComponent } from '../../../forms/sign-up-form/sign-up-form.component';
import { FormValidationErrorComponent } from '../../../shared/validation/form-validation-error/form-validation-error.component';
import { Router } from '@angular/router';
import { CustomerService } from '../../../../services/customer.service';
import { ShoppingCartResponseDto } from '../../../dto/shoppingcart/shoppingcart-response-dto';

@Component({
  selector: 'app-authentication',
  standalone: true,
  imports: [DividerModule, ButtonModule, InputTextModule, FormValidationErrorComponent, CommonModule, LoginFormComponent, SignUpFormComponent],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent implements OnInit {
  signUp = false
  connected = false;
  constructor(private customerService: CustomerService,
    private router: Router) { }

  ngOnInit(): void {
    this.signUp = false;
    this.connected = this.customerService.isLoggedIn();
    if (this.connected) {
      this.customerService.updateCustomerShoppingCart()?.subscribe({
        next: (r) => {
          if (r) {
            this.router.navigate(['/steps/payment']);
          }
        }
      })
    }
  }
  navigateToNext(connected: any) {
    if (connected) {
      this.customerService.updateCustomerShoppingCart()?.subscribe({
        next: (r) => {
          if (r) {
            this.router.navigate(['/steps/payment']);
          }
        }
      })
    }
  }
  navigateToPrevious() {
    this.router.navigate(['/steps/my-shoppingcart'])
  }
  showSignUp() {
    this.signUp = true;
  }
}
