import { BaseResponse } from "./base-response";

export class CommandResponse extends BaseResponse {
    validationErrors: string[] = [];
    constructor(message?: string, error?: boolean, validationErrors?: string[]) {
        super(message, error);
        this.validationErrors = validationErrors ?? this.validationErrors;
    }
}