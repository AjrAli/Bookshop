import { Injectable } from "@angular/core";
import { environment } from "../app/environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { GetByIdResponse } from "../app/dto/handler-response/common/get-by-id-response";
import { GetAllResponse } from "../app/dto/handler-response/common/get-all-response";

@Injectable()
export class CommonApiService {
    protected apiUrl: string = environment.apiUrl;

    constructor(protected http: HttpClient) { }

    getById(Id: number): Observable<GetByIdResponse> {
        return this.http.get<GetByIdResponse>(`${this.apiUrl}/${Id}`);
    }
    getAll(): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}`);
    }
}