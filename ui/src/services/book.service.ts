import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../app/environments/environment";

@Injectable()
export class BookService {
    private readonly apiUrl = environment.apiUrl + '/book';

    constructor(private http: HttpClient) { }

    getById(bookId: number): Observable<any> {
        return this.http.get(`${this.apiUrl}/${bookId}`);
    }
    getAll(): Observable<any> {
        return this.http.get(`${this.apiUrl}`);
    }
}