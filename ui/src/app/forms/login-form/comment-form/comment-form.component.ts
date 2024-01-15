import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FormValidationErrorComponent } from '../../../shared/validation/form-validation-error/form-validation-error.component';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RatingModule } from 'primeng/rating';
import { CommentResponseDto } from '../../../dto/book/comment/comment-response-dto';
import { BookService } from '../../../../services/book.service';
import { CommentDto } from '../../../dto/book/comment/comment-dto';

@Component({
  selector: 'app-comment-form',
  standalone: true,
  imports: [ButtonModule, InputTextModule, FormValidationErrorComponent, ReactiveFormsModule, CommonModule, InputTextareaModule, RatingModule],
  templateUrl: './comment-form.component.html',
  styleUrl: './comment-form.component.css'
})
export class CommentFormComponent implements OnInit {
  commentForm!: FormGroup;
  @Input() bookId: number | undefined;
  @Output() newCommentCreated = new EventEmitter<CommentResponseDto>();

  constructor(private bookService: BookService) { }

  ngOnInit(): void {
    this.commentForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      rating: new FormControl(1, [Validators.required, Validators.max(5)]),
      content: new FormControl('', [Validators.required, Validators.maxLength(1000)])
    });
  }
  onSubmit() {
    const title = this.commentForm.get('title')?.value;
    const rating = this.commentForm.get('rating')?.value;
    const content = this.commentForm.get('content')?.value;
    if (this.bookId && title && rating && content) {
      const commentDto = new CommentDto({ title: title, rating: rating, content: content, bookId: this.bookId });
      this.bookService.addComment(commentDto).subscribe({
        next: (response) => {
          this.newCommentCreated.emit(response);
        }
      });
    }
  }
}
