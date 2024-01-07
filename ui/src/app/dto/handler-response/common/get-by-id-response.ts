import { BaseResponse } from "../../response/base-response";

export class GetByIdResponse extends BaseResponse {

    dto: any;

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.dto = new Object();
    }
}