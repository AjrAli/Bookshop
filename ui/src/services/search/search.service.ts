import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../app/environments/environment';
import { BookResponseDto } from '../../app/dto/book/book-response-dto';

@Injectable()
export class SearchService {
    private readonly apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    getSearchResults(keyword: string): Observable<BookResponseDto[]> {
        return this.http.get<BookResponseDto[]>(`${this.apiUrl}/search/book/${keyword}`);
    }
}