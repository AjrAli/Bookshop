import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, FormsModule, AbstractControl, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CustomerService } from '../../../services/customer.service';
import { ToastService } from '../../../services/toast.service';
import { ValidationErrorResponse } from '../../dto/response/error/validation-error-response';
import { FormValidationErrorComponent, PasswordMatchValidator } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { CheckboxModule } from 'primeng/checkbox';
import { AddressDto, CustomerDto } from '../../dto/customer/customer-dto';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, CheckboxModule, FormsModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent implements OnInit {
  username: string = '';
  password: string = '';
  confirmPassword: string = '';
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  isSameAddress = true;
  shipstreet: string = '';
  shipcity: string = '';
  shippostalCode: string = '';
  shipcountry: string = '';
  shipstate: string = '';
  billstreet: string = '';
  billcity: string = '';
  billpostalCode: string = '';
  billcountry: string = '';
  billstate: string = '';
  newCustomer: CustomerDto = new CustomerDto();
  shippingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  billingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  loginForm!: FormGroup;
  errorResponse!: ValidationErrorResponse;

  constructor(private customerService: CustomerService,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService,
    private fb: FormBuilder) { }

  ngOnInit(): void {

    if (this.customerService.isLoggedIn()) {
      this.router.navigate(['']);
    }

    this.loginForm = this.fb.group({
      username: this.createControl(''),
      password: this.createControl('', [Validators.minLength(4)]),
      confirmPassword: this.createControl('', [Validators.minLength(4)]),
      firstName: this.createControl(''),
      lastName: this.createControl(''),
      email: this.createControl('', [Validators.email]),
      isSameAddress: this.createControl(false),
      shipstreet: this.createControl(''),
      shipcity: this.createControl(''),
      shippostalCode: this.createControl(''),
      shipcountry: this.createControl(''),
      shipstate: this.createControl(''),
      billstreet: this.createControl(''),
      billcity: this.createControl(''),
      billpostalCode: this.createControl(''),
      billcountry: this.createControl(''),
      billstate: this.createControl('')
    }, {
      validators: [Validators.required, Validators.maxLength(100), PasswordMatchValidator.match('password', 'confirmPassword')]
    });
    this.subscribeToIsSameAddressChanges();
  }

  private createControl(initialValue: any, validators: any[] = []): AbstractControl {
    return this.fb.control({ value: initialValue, disabled: false }, validators);
  }

  private subscribeToIsSameAddressChanges(): void {
    const billControls = ['billstreet', 'billcity', 'billpostalCode', 'billcountry', 'billstate'];

    this.loginForm?.get('isSameAddress')?.valueChanges.subscribe((value) => {
      billControls.forEach(controlName => {
        const control = this.loginForm.get(controlName);
        if (control) {
          if (value) {
            control.clearValidators();
            control.disable();
          } else {
            control.enable();
            control.setValidators([Validators.required, Validators.maxLength(100)]);
          }
          control.updateValueAndValidity();
        }
      });
    });
  }
  onSubmit() {

    const username = this.loginForm.get('username')?.value;
    const password = this.loginForm.get('password')?.value;
    const confirmPassword = this.loginForm.get('confirmPassword')?.value;
    const firstName = this.loginForm.get('firstName')?.value;
    const lastName = this.loginForm.get('lastName')?.value;
    const email = this.loginForm.get('email')?.value;
    const isSameAddress = this.loginForm.get('isSameAddress')?.value;
    const shipstreet = this.loginForm.get('shipstreet')?.value;
    const shipcity = this.loginForm.get('shipcity')?.value;
    const shippostalCode = this.loginForm.get('shippostalCode')?.value;
    const shipcountry = this.loginForm.get('shipcountry')?.value;
    const shipstate = this.loginForm.get('shipstate')?.value;
    const billstreet = this.loginForm.get('billstreet')?.value;
    const billcity = this.loginForm.get('billcity')?.value;
    const billpostalCode = this.loginForm.get('billpostalCode')?.value;
    const billcountry = this.loginForm.get('billcountry')?.value;
    const billstate = this.loginForm.get('billstate')?.value;


    this.shippingAddress = new AddressDto(0, shipstreet, shipcity, shippostalCode, shipcountry, shipstate);
    if (isSameAddress)
      this.billingAddress = this.shippingAddress;
    else
      this.billingAddress = new AddressDto(0, billstreet, billcity, billpostalCode, billcountry, billstate);
    if (this.shippingAddress && this.billingAddress) {
      this.newCustomer = new CustomerDto(username, password, confirmPassword, firstName, lastName, 0, this.shippingAddress, 0, this.billingAddress, email, true);
      this.customerService.createCustomer(this.newCustomer).subscribe({
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
}
