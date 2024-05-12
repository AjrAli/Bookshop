import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CommonApiService } from "./common-api.service";

@Injectable()
export class AuthorService extends CommonApiService {

    constructor(http: HttpClient) {
        super(http);
        this.apiUrl += '/authors';
    }
}