import { Component, OnInit } from '@angular/core';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CustomerService } from '../../../services/customer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormValidationErrorComponent } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { CommonModule } from '@angular/common';
import { LoginFormComponent } from '../../forms/login-form/login-form.component';
import { SignUpFormComponent } from '../../forms/sign-up-form/sign-up-form.component';

@Component({
  selector: 'app-login-proposale',
  standalone: true,
  imports: [DividerModule, ButtonModule, InputTextModule, FormValidationErrorComponent, CommonModule, LoginFormComponent, SignUpFormComponent],
  templateUrl: './login-proposale.component.html',
  styleUrl: './login-proposale.component.css'
})
export class LoginProposaleComponent implements OnInit {
  signUp = false
  constructor(private customerService: CustomerService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.signUp = false;
    if (this.customerService.isLoggedIn()) {
      this.navigateToNext();
    }
  }
  navigateRequest(connected: any) {
    if (connected) {
      this.navigateToNext();
    }
  }
  navigateToNext(){
    const returnUrlParam = this.getPreviousUrlRequest();
    this.router.navigate([returnUrlParam ?? '']);
  }
  getPreviousUrlRequest(): string | null{
    return this.route?.snapshot?.queryParamMap?.get('returnUrl');
  }
  showSignUp() {
    this.signUp = true;
  }
}
