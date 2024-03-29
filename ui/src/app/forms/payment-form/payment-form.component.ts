import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { FormValidationErrorComponent } from '../../shared/validation/form-validation-error/form-validation-error.component';
import { CreditCards, PaymentInformation } from '../../../services/customer/customer-data.service';
import { DropdownModule } from 'primeng/dropdown';
import { InputMaskModule } from 'primeng/inputmask';

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, CheckboxModule, FormsModule, DropdownModule, InputMaskModule],
  templateUrl: './payment-form.component.html',
  styleUrl: './payment-form.component.css'
})
export class PaymentFormComponent implements OnInit {
  cardholderName: string = '';
  methodOfPayment: string = CreditCards.AmericanExpress;
  cardholderNumber: string = '';
  date: string = '';
  cvv: string = '';
  paymentForm!: FormGroup;
  creditCardsOptions: { label: string, value: string }[] = Object.keys(CreditCards).map(key => ({ label: key, value: (CreditCards as any)[key] }));

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      cardholderName: new FormControl(this.cardholderName, [Validators.required]),
      methodOfPayment: new FormControl(this.methodOfPayment, [Validators.required]),
      cardholderNumber: new FormControl(this.cardholderNumber, [Validators.required]),
      date: new FormControl(this.date, [Validators.required]),
      cvv: new FormControl(this.cvv, [Validators.required])
    });
  }

  getFormData(): PaymentInformation | undefined {
    if (this.paymentForm.valid) {
      const cardholderName = this.paymentForm.get('cardholderName')?.value;
      const methodOfPayment = this.paymentForm.get('methodOfPayment')?.value;
      const cardholderNumber = this.paymentForm.get('cardholderNumber')?.value;
      const date = this.paymentForm.get('date')?.value;
      const cvv = this.paymentForm.get('cvv')?.value;
      const paymentInformation: PaymentInformation = {
        cardholderName: cardholderName,
        methodOfPayment: methodOfPayment,
        cardholderNumber: cardholderNumber,
        date: date,
        cvv: cvv
      }
      return paymentInformation;
    }
    return;
  }
}
