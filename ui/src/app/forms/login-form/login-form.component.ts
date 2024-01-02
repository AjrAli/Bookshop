import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { CustomerService } from '../../../services/customer.service';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FormValidationErrorComponent } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { ValidationErrorResponse } from '../../dto/response/error/validation-error-response';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.css'
})
export class LoginFormComponent implements OnInit {
  username: string = '';
  password: string = '';
  loginForm!: FormGroup;
  errorResponse!: ValidationErrorResponse;
  @Output() connected = new EventEmitter<boolean>();

  constructor(private customerService: CustomerService) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(this.username, [Validators.required, Validators.maxLength(100)]),
      password: new FormControl(this.password, [Validators.required, Validators.maxLength(100)])
    });
  }
  onSubmit() {
    this.customerService.authenticate(this.loginForm.get('username')?.value, this.loginForm.get('password')?.value).subscribe({
      next: (response) => {
        this.connected.emit(response);
      }
    });
  }
}
