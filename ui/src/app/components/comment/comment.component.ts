import { CommonModule } from '@angular/common';
import { Component, HostListener, Input, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RatingModule } from 'primeng/rating';
import { Subject, Subscription, debounceTime } from 'rxjs';
import { BookService } from '../../../services/book.service';
import { ToastService } from '../../../services/toast.service';
import { CommentResponseDto } from '../../dto/book/comment/comment-response-dto';
import { CustomerDataService } from '../../../services/customer/customer-data.service';
import { CommentFormComponent } from '../../forms/login-form/comment-form/comment-form.component';
import { CustomerResponseDto } from '../../dto/customer/customer-response-dto';

@Component({
  selector: 'app-comment',
  standalone: true,
  imports: [CommonModule, ButtonModule, RatingModule, FormsModule, CommentFormComponent],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent implements OnInit, OnDestroy {
  showComments = 5;
  @Input() comments: CommentResponseDto[] | undefined;
  @Input() bookId: number | undefined;
  showForm: boolean | null = null;
  customer: CustomerResponseDto | null = null;
  private customerSubscription: Subscription | undefined;
  private scrollSubject = new Subject<Event>();
  private commentSubscription: Subscription | undefined;
  private commentApiSubscription: Subscription | undefined;
  private urlSubscription: Subscription | undefined;

  constructor(private router: Router,
    private bookService: BookService,
    private route: ActivatedRoute,
    private customerDataService: CustomerDataService,
    private toastService: ToastService) { }
  ngOnDestroy(): void {
    this.commentSubscription?.unsubscribe();
    this.urlSubscription?.unsubscribe();
    this.customerSubscription?.unsubscribe();
    this.commentApiSubscription?.unsubscribe();
  }
  ngOnInit(): void {
    this.customerSubscription = this.customerDataService.getCustomerObservable().subscribe({
      next: (customer) => {
        if (customer) {
          this.customer = customer;
          const commentWithSameUser = this.comments?.find(x => x.userName === customer.userName);
          this.showForm = true && !commentWithSameUser;
        } else {
          this.showForm = null;
        }
      }
    });
    this.commentSubscription = this.scrollSubject.pipe(debounceTime(200)).subscribe({
      next: () => {
        const bookViewElement = document.body.querySelector("#bookView");
        if (bookViewElement && window.innerHeight + document.documentElement.scrollTop >= (bookViewElement.scrollHeight + 300)) {
          this.showComments += 5;
        }
      }
    });
  }
  navigateToLogin() {
    this.urlSubscription = this.route.url.subscribe({
      next: (response) => {
        if (response && response.length > 0) {
          const urlToChange = `/${response.join("/")}`;
          if (urlToChange)
            this.router.navigate(['/login'], { queryParams: { returnUrl: urlToChange } });
        }
      }
    })
  }
  getCommentCreated(comment: any) {
    if (comment) {
      this.comments?.push(comment);
      this.showForm = false;
    }
  }
  removeComment(id: number) {
    if (id) {
      this.bookService.deleteComment(id).subscribe({
        next: (response) => {
          if (response && this.comments && this.comments.length > 0) {
            const commentIndexToDelete = this.comments.findIndex(x => x.id === id);
            this.comments.splice(commentIndexToDelete, 1)
            this.showForm = true;
          }
        }
      });
    }
  }
  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: Event) {
    // debounce for 200 milliseconds
    this.scrollSubject.next(event);
  }

}
