import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { CustomerResponseDto } from "../../app/dto/customer/customer-response-dto";

export enum CreditCards {
    AmericanExpress = 'AmericanExpress',
    Discover = 'Discover',
    Mastercard = 'Mastercard',
    Visa = 'Visa'
}
export interface PaymentInformation {
    methodOfPayment: CreditCards;
    cardholderName: string;
    cardholderNumber: string;
    date: string;
    cvv: string;
}

@Injectable()
export class CustomerDataService {
    private paymentInformation: PaymentInformation | null = null;
    private paymentInformationSubject: BehaviorSubject<PaymentInformation | null> = new BehaviorSubject<PaymentInformation | null>(null);
    private customer: CustomerResponseDto | null = null;
    private customerSubject: BehaviorSubject<CustomerResponseDto | null> = new BehaviorSubject<CustomerResponseDto | null>(null);

    getPaymentInformation(): PaymentInformation | null {
        return this.paymentInformation;
    }
    getPaymentInformationObservable(): Observable<PaymentInformation | null> {
        return this.paymentInformationSubject.asObservable();
    }
    getCustomer(): CustomerResponseDto | null {
        return this.customer;
    }
    getCustomerObservable(): Observable<CustomerResponseDto | null> {
        return this.customerSubject.asObservable();
    }

    setCustomer(customer: CustomerResponseDto) {
        if (!customer) {
            return;
        }
        this.customer = new CustomerResponseDto(customer);
        this.customerSubject.next(this.customer);
    }
    setPaymentInformation(paymentInformation: PaymentInformation) {
        if (!paymentInformation) {
            return;
        }
        this.paymentInformation = paymentInformation;
        this.paymentInformationSubject.next(this.paymentInformation);
    }
    resetPaymentInformation() {
        this.paymentInformation = null;
        this.paymentInformationSubject.next(null as PaymentInformation | null);
    }
    resetCustomer() {
        this.customer = null;
        this.customerSubject.next(null as CustomerResponseDto | null);
        this.paymentInformation = null;
        this.paymentInformationSubject.next(null as PaymentInformation | null);
    }

}