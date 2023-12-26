import { ErrorResponse } from "./error-response";

export class ValidationErrorResponse extends ErrorResponse {
    validationErrors: string[];
    constructor(message?: string, validationErrors?:string[]){
      super(message);
      this.validationErrors = validationErrors ?? [];
    }
  }