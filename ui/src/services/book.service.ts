import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CommonApiService } from "./common-api.service";
import { GetAllResponse } from "../app/dto/handler-response/common/get-all-response";
import { Observable } from "rxjs";

@Injectable()
export class BookService extends CommonApiService {

    constructor(http: HttpClient) {
        super(http);
        this.apiUrl += '/book';
    }
    getBooksByAuthorId(authorId: number): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}/author/${authorId}`);
    }
    getBooksByCategoryId(categoryId: number): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}/category/${categoryId}`);
    }
}