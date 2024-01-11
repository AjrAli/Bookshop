import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { ReactiveFormsModule, FormGroup, Validators, FormsModule, AbstractControl, FormBuilder, FormControl } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CustomerService } from '../../../services/customer.service';
import { FormValidationErrorComponent, PasswordMatchValidator } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { CheckboxModule } from 'primeng/checkbox';
import { AddressDto, CustomerDto } from '../../dto/customer/customer-dto';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-sign-up-form',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, CheckboxModule, FormsModule],
  templateUrl: './sign-up-form.component.html',
  styleUrl: './sign-up-form.component.css'
})
export class SignUpFormComponent implements OnInit, OnDestroy {
  newCustomer: CustomerDto = new CustomerDto();
  shippingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  billingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  loginForm!: FormGroup;
  @Output() connected = new EventEmitter<boolean>();
  private customerSubscription: Subscription | undefined;
  private loginSubscription: Subscription | undefined;
  constructor(private customerService: CustomerService,
    private fb: FormBuilder) { }

  ngOnDestroy(): void {
    this.customerSubscription?.unsubscribe();
    this.loginSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
      confirmPassword: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(100)]),
      firstName: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      lastName: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      email: new FormControl('', [Validators.required, Validators.maxLength(100), Validators.email]),
      isSameAddress: new FormControl(false),
      shipstreet: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      shipcity: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      shippostalCode: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      shipcountry: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      shipstate: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      billstreet: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      billcity: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      billpostalCode: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      billcountry: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      billstate: new FormControl('', [Validators.required, Validators.maxLength(100)])
    }, {
      validators: [PasswordMatchValidator.match('password', 'confirmPassword')]
    });
    this.subscribeToIsSameAddressChanges();
  }
  private subscribeToIsSameAddressChanges(): void {
    const billControls = ['billstreet', 'billcity', 'billpostalCode', 'billcountry', 'billstate'];

    this.loginSubscription = this.loginForm?.get('isSameAddress')?.valueChanges.subscribe((value) => {
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


    this.shippingAddress = new AddressDto({ id: 0, street: shipstreet, city: shipcity, postalCode: shippostalCode, country: shipcountry, state: shipstate });
    if (isSameAddress)
      this.billingAddress = this.shippingAddress;
    else
      this.billingAddress = new AddressDto({ id: 0, street: billstreet, city: billcity, postalCode: billpostalCode, country: billcountry, state: billstate });
    if (this.shippingAddress && this.billingAddress) {
      this.newCustomer = new CustomerDto({
        username: username,
        password: password,
        confirmPassword: confirmPassword,
        firstName: firstName,
        lastName: lastName,
        shippingAddressId: 0,
        shippingAddress: this.shippingAddress,
        billingAddressId: 0,
        billingAddress: this.billingAddress,
        email: email,
        emailConfirmed: true
      });
      this.customerSubscription = this.customerService.createCustomer(this.newCustomer).subscribe({
        next: (response) => {
          this.connected.emit(response);
        }
      });
    }
  }
}
