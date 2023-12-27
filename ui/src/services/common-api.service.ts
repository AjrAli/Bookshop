import { Injectable } from "@angular/core";
import { environment } from "../app/environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class CommonApiService {
    protected apiUrl: string = environment.apiUrl;

    constructor(protected http: HttpClient) { }

    getById(Id: number): Observable<any> {
        return this.http.get(`${this.apiUrl}/${Id}`);
    }
    getAll(): Observable<any> {
        return this.http.get(`${this.apiUrl}`);
    }
}