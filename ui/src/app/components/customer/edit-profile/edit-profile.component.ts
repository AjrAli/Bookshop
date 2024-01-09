import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators, AbstractControl, FormControl } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { CustomerService } from '../../../../services/customer.service';
import { AddressDto } from '../../../dto/customer/customer-dto';
import { ValidationErrorResponse } from '../../../dto/response/error/validation-error-response';
import { FormValidationErrorComponent } from '../../../shared/validation/form-validation-error/form-validation-error.component';
import { EditProfileDto } from '../../../dto/customer/edit-profile-dto';
import { CustomerDataService } from '../../../../services/customer/customer-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, CheckboxModule, FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent implements OnInit {
  updatedCustomer: EditProfileDto = new EditProfileDto();
  shippingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  billingAddress: AddressDto = new AddressDto(); // Assuming AddressDto is another class
  loginForm!: FormGroup;
  errorResponse!: ValidationErrorResponse;

  constructor(private customerService: CustomerService,
    private customerDataService: CustomerDataService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.customerDataService.getCustomerObservable().subscribe({
      next: (customer) => {
        const shipArrayAddress = customer?.shippingAddress?.split(", ") ?? [''];
        const billArrayAddress = customer?.billingAddress?.split(", ") ?? [''];
        this.loginForm = this.fb.group({
          firstName: new FormControl(customer?.firstName, [Validators.required, Validators.maxLength(100)]),
          lastName: new FormControl(customer?.lastName, [Validators.required, Validators.maxLength(100)]),
          isSameAddress: new FormControl(false),
          shipstreet: new FormControl(shipArrayAddress[0], [Validators.required, Validators.maxLength(100)]),
          shipcity: new FormControl(shipArrayAddress[1], [Validators.required, Validators.maxLength(100)]),
          shippostalCode: new FormControl(shipArrayAddress[2], [Validators.required, Validators.maxLength(100)]),
          shipstate: new FormControl(shipArrayAddress[3], [Validators.required, Validators.maxLength(100)]),
          shipcountry: new FormControl(shipArrayAddress[4], [Validators.required, Validators.maxLength(100)]),
          billstreet: new FormControl(billArrayAddress[0], [Validators.required, Validators.maxLength(100)]),
          billcity: new FormControl(billArrayAddress[1], [Validators.required, Validators.maxLength(100)]),
          billpostalCode: new FormControl(billArrayAddress[2], [Validators.required, Validators.maxLength(100)]),
          billstate: new FormControl(billArrayAddress[3], [Validators.required, Validators.maxLength(100)]),
          billcountry: new FormControl(billArrayAddress[4], [Validators.required, Validators.maxLength(100)]),
        });
        this.subscribeToIsSameAddressChanges();
      }
    });
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
    const firstName = this.loginForm.get('firstName')?.value;
    const lastName = this.loginForm.get('lastName')?.value;
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
      this.updatedCustomer = new EditProfileDto({
        firstName: firstName,
        lastName: lastName,
        shippingAddressId: 0,
        shippingAddress: this.shippingAddress,
        billingAddressId: 0,
        billingAddress: this.billingAddress
      });
      this.customerService.editProfile(this.updatedCustomer).subscribe({
        next: (response) => {
          if (response)
            this.router.navigate(['/customer/view-profile']);
        }
      });
    }
  }
}