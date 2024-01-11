import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CustomerService } from '../../../../services/customer.service';
import { FormValidationErrorComponent, PasswordMatchValidator } from '../../../shared/validation/form-validation-error/form-validation-error.component';
import { EditPasswordDto } from '../../../dto/customer/edit-password-dto';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-edit-password',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './edit-password.component.html',
  styleUrl: './edit-password.component.css'
})
export class EditPasswordComponent implements OnInit, OnDestroy {
  editPassword: EditPasswordDto = new EditPasswordDto();
  loginForm!: FormGroup;
  private customerSubscription: Subscription | undefined;
  constructor(private customerService: CustomerService,
    private router: Router,
    private fb: FormBuilder) { }


  ngOnDestroy(): void {
    this.customerSubscription?.unsubscribe();
  }
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      password: new FormControl("", [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
      confirmPassword: new FormControl("", [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
      newPassword: new FormControl("", [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
      confirmNewPassword: new FormControl("", [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
    }, {
      validators: [PasswordMatchValidator.match('password', 'confirmPassword'), PasswordMatchValidator.match('newPassword', 'confirmNewPassword')]
    });
  }


  onSubmit() {
    const password = this.loginForm.get('password')?.value;
    const confirmPassword = this.loginForm.get('confirmPassword')?.value;
    const newPassword = this.loginForm.get('newPassword')?.value;
    const confirmNewPassword = this.loginForm.get('confirmNewPassword')?.value;
    this.editPassword = new EditPasswordDto({
      password: password,
      confirmPassword: confirmPassword,
      newPassword: newPassword,
      confirmNewPassword: confirmNewPassword
    });
    this.customerSubscription = this.customerService.editPassword(this.editPassword).subscribe({
      next: (response) => {
        if (response)
          this.router.navigate(['/customer/view-profile']);
      }
    });
  }
}

