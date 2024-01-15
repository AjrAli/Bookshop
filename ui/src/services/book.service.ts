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
        this.apiUrl += '/book';
    }
    getBooksByAuthorId(authorId: number): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}/author/${authorId}`);
    }
    getBooksByCategoryId(categoryId: number): Observable<GetAllResponse> {
        return this.http.get<GetAllResponse>(`${this.apiUrl}/category/${categoryId}`);
    }
    addComment(comment: CommentDto): Observable<CommentResponseDto> {
        return this.http.post<CommentCommandResponse>(`${this.apiUrl}/add-comment-book`, comment).pipe(tap({
            next: (r) => this.handleCommentResponse(r),
            error: (e) => this.handleCommentError(e),
            complete: () => console.info('complete')
        }), map(response => response.comment));
    }
    updateComment(comment: CommentDto): Observable<CommentResponseDto> {
        return this.http.post<CommentCommandResponse>(`${this.apiUrl}/update-comment-book`, comment).pipe(tap({
            next: (r) => this.handleCommentResponse(r),
            error: (e) => this.handleCommentError(e),
            complete: () => console.info('complete')
        }), map(response => response.comment));
    }
    deleteComment(id: number): Observable<boolean> {
        return this.http.post<CommentCommandResponse>(`${this.apiUrl}/delete-comment-book`, id).pipe(tap({
            next: (r) => this.toastService.showSuccess(r.message),
            error: (e) => this.handleCommentError(e),
            complete: () => console.info('complete')
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