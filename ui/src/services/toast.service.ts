import { Injectable } from '@angular/core';
import { ErrorResponse } from '../app/dto/response/error/error-response';
import { CommandResponse } from '../app/dto/response/command-response';
import { MessageService } from 'primeng/api';

@Injectable()
export class ToastService {
  constructor(private messageService: MessageService) { }

  showSuccess(message: string): void {
    this.showNotification('success', message, 'Success');
  }

  showValidationError(commandResponse?: Partial<CommandResponse>): void {
    const errorMessageArray = [commandResponse?.message, ...(commandResponse?.validationErrors ?? [])];
    if (errorMessageArray)
      errorMessageArray.forEach((message) => {
        if (message) {
          this.showNotification('error', message, 'Error');
        }
      });
  }
  showError(errorResponse: ErrorResponse): void {
    const errorMessage: string = errorResponse?.message;

    if (errorMessage) {
      this.showNotification('error', errorMessage, 'Error');
    }
  }
  showSimpleError(message: string): void {
    this.showNotification('error', message, 'Error');
  }
  private showNotification(type: 'success' | 'error', message: string, title: string): void {
    this.messageService.add({
      key: 'bc',
      severity: type,
      summary: title,
      detail: message,
    });
  }
}
