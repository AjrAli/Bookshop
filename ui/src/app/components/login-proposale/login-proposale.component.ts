import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { DividerModule } from 'primeng/divider';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastService } from '../../../services/toast.service';
import { ValidationErrorResponse } from '../../dto/response/error/validation-error-response';
import { BaseResponse } from '../../dto/response/base-response';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomerService } from '../../../services/customer.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormValidationErrorComponent } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-proposale',
  standalone: true,
  imports: [DividerModule, ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule ],
  templateUrl: './login-proposale.component.html',
  styleUrl: './login-proposale.component.css'
})
export class LoginProposaleComponent implements OnInit {
  username: string = '';
  password: string = '';
  loginForm!: FormGroup;
  errorResponse!: ValidationErrorResponse;

  constructor(private customerService: CustomerService,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService){}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(this.username, [Validators.required, Validators.maxLength(100)]),
      password: new FormControl(this.password, [Validators.required, Validators.maxLength(100)])
    });
    if (this.customerService.isLoggedIn()) {
      this.router.navigate(['']);
    }
  }
  navigateToSignUp(){
    this.router.navigate(['/sign-up']);
  }
  onSubmit() {
    this.customerService.authenticate(this.loginForm.get('username')?.value, this.loginForm.get('password')?.value).subscribe({
      next: (r) => {
        if (r.token) {
          this.customerService.setToken(r.token);
          const returnUrl = this.route.snapshot.queryParams['returnUrl'];
          this.toastService.showSuccess(r.message);
          this.router.navigateByUrl(returnUrl ?? '');
        } else {
          this.toastService.showSimpleError('Invalid credentials');
        }
      },
      error: (e) => {
        this.toastService.showError(e.error as ValidationErrorResponse);
        this.toastService.showError(e as ValidationErrorResponse);
      },
      complete: () => console.info('complete')
    });
  }
}
