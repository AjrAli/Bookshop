import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Optional, Output } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FormValidationErrorComponent } from '../../../shared/validation/form-validation-error/form-validation-error.component';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RatingModule } from 'primeng/rating';
import { CommentResponseDto } from '../../../dto/book/comment/comment-response-dto';
import { BookService } from '../../../../services/book.service';
import { CommentDto } from '../../../dto/book/comment/comment-dto';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-comment-form',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, InputTextareaModule, RatingModule],
  templateUrl: './comment-form.component.html',
  styleUrl: './comment-form.component.css'
})
export class CommentFormComponent implements OnInit {
  commentForm!: FormGroup;
  comment: CommentResponseDto | undefined;
  @Input() bookId: number | undefined;
  @Output() newCommentCreated = new EventEmitter<CommentResponseDto>();

  constructor(private bookService: BookService, @Optional() private dynamicDialogConfig: DynamicDialogConfig, @Optional() private modalRefPassed: DynamicDialogRef) {
    if (this.dynamicDialogConfig?.data)
      this.comment = new CommentResponseDto(this.dynamicDialogConfig?.data);
    else {
      this.comment = undefined;
    }
  }

  ngOnInit(): void {
    this.commentForm = new FormGroup({
      title: new FormControl(this.comment?.title ?? '', [Validators.required, Validators.maxLength(100)]),
      rating: new FormControl(this.comment?.rating ?? 1, [Validators.required, Validators.max(5)]),
      content: new FormControl(this.comment?.content ?? '', [Validators.required, Validators.maxLength(1000)])
    });
  }
  onSubmit() {
    const title = this.commentForm.get('title')?.value;
    const rating = this.commentForm.get('rating')?.value;
    const content = this.commentForm.get('content')?.value;
    if (!this.comment) {
      this.addComment(this.bookId, title, rating, content);
    } else {
      this.updateComment(this.comment, title, rating, content);
    }
  }
  addComment(bookId: number | undefined, title: string, rating: number, content: string) {
    if (bookId && title && rating && content) {
      const commentDto = new CommentDto({ title: title, rating: rating, content: content, bookId: bookId });
      this.bookService.addComment(commentDto).subscribe({
        next: (response) => {
          this.newCommentCreated.emit(response);
        }
      });
    }
  }
  updateComment(comment: CommentResponseDto, title: string, rating: number, content: string) {
    comment.title = title;
    comment.rating = rating;
    comment.content = content;
    const commentDto = new CommentDto({ id: comment.id, title: comment.title, rating: comment.rating, content: comment.content });
    this.bookService.updateComment(commentDto).subscribe({
      next: (response) => {
        this.modalRefPassed?.close(new CommentResponseDto(response));
      }
    });
  }

}
