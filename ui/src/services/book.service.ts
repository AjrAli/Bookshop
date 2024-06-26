import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CommonApiService } from "./common-api.service";
import { GetAllResponse } from "../app/dto/handler-response/common/get-all-response";
import { Observable, map, tap } from "rxjs";
import { ToastService } from "./toast.service";
import { CommentResponseDto } from "../app/dto/book/comment/comment-response-dto";
import { CommentCommandResponse } from "../app/dto/handler-response/book/comment/comment-command.response";
import { CommentDto } from "../app/dto/book/comment/comment-dto";

@Injectable()
export class BookService extends CommonApiService {

    constructor(http: HttpClient,
        private toastService: ToastService) {
        super(http);
        this.apiUrl += '/books';
    }
    getBooksByAuthorId(authorId: number): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}/by-author/${authorId}`);
    }
    getBooksByCategoryId(categoryId: number): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}/by-category/${categoryId}`);
    }
    addComment(comment: CommentDto, bookId: number): Observable<CommentResponseDto> {
        return this.http.post<CommentCommandResponse>(`${this.apiUrl}/${bookId}/comments`, comment).pipe(tap({
            next: (r) => this.handleCommentResponse(r),
            error: (e) => this.handleCommentError(e)
        }), map(response => response.comment));
    }
    updateComment(comment: CommentDto, id: number): Observable<CommentResponseDto> {
        return this.http.put<CommentCommandResponse>(`${this.apiUrl}/comments/${id}`, comment).pipe(tap({
            next: (r) => this.handleCommentResponse(r),
            error: (e) => this.handleCommentError(e)
        }), map(response => response.comment));
    }
    deleteComment(id: number): Observable<boolean> {
        return this.http.delete<CommentCommandResponse>(`${this.apiUrl}/comments/${id}`).pipe(tap({
            next: (r) => this.toastService.showSuccess(r.message),
            error: (e) => this.handleCommentError(e)
        }), map(response => response.success));
    }

    private handleCommentResponse(response: CommentCommandResponse): void {
        if (response.comment) {
            this.toastService.showSuccess(response.message);
        } else {
            this.toastService.showValidationError(response);
        }
    }

    private handleCommentError(error: any): void {
        this.toastService.showError(error.error);
        this.toastService.showError(error);
    }
}