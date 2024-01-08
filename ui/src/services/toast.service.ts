import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ValidationErrorResponse } from '../app/dto/response/error/validation-error-response';
import { ErrorResponse } from '../app/dto/response/error/error-response';

@Injectable()
export class ToastService {
  constructor(private toastr: ToastrService) { }

  showSuccess(message: string): void {
    this.showNotification('success', message, 'Success', 3000);
  }

  showValidationError(validationError?: Partial<ValidationErrorResponse>): void {
    const errorMessageArray = [validationError?.message, ...(validationError?.validationErrors ?? [])];
    if (errorMessageArray)
      errorMessageArray.forEach((message) => {
        if (message) {
          this.showNotification('error', message, 'Error', 5000);
        }
      });
  }
  showError(errorResponse: ErrorResponse): void {
    const errorMessage: string = errorResponse?.message;

    if (errorMessage) {
      this.showNotification('error', errorMessage, 'Error', 5000);
    }
  }
  showSimpleError(message: string): void {
    this.showNotification('error', message, 'Error', 5000);
  }
  private showNotification(type: 'success' | 'error', message: string, title: string, duration: number): void {
    const options = {
      positionClass: 'toast-bottom-center',
      timeOut: duration
    };

    if (type === 'success') {
      this.toastr.success(message, title, options);
    } else {
      this.toastr.error(message, title, options);
    }
  }
}
