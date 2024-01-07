import { BaseResponse } from "../../response/base-response";

export class GetAllResponse extends BaseResponse {

    listDto: any[];

    constructor(message?: string, error?: boolean) {
        super(message, error);
        this.listDto = [];
    }
}