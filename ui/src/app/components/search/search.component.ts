import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ListCardsComponent } from '../../body/list-cards/list-cards.component';
import { BookResponseDto } from '../../dto/book/book-response-dto';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchStateService } from '../../../services/search/search-state.service';
import { SearchService } from '../../../services/search/search.service';
import { ToastService } from '../../../services/toast.service';
import { Subject, catchError, debounceTime, distinctUntilChanged, of, startWith, takeUntil } from 'rxjs';
import { ErrorResponse } from '../../dto/response/error/error-response';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, ListCardsComponent],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent implements OnInit {
  books: BookResponseDto[] | undefined;
  keyword: string = '';
  private destroy$: Subject<void> = new Subject<void>();
  private searchKeyword$ = new Subject<string>();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private searchService: SearchService,
    private searchStateService: SearchStateService,
    private toastService: ToastService
  ) {
  }

  ngOnInit(): void {
    this.initializeSearchStateService();
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  private initializeSearchStateService(): void {
    this.route.queryParams.subscribe({
      next: (params: any) => {
        if (params?.keyword) {
          this.keyword = decodeURIComponent(params.keyword ?? '');
          this.searchStateService.setSearchKeyword(params.keyword ?? '');
        }
        // Subscribe to changes in the search keyword in the SearchStateService
        this.searchStateService.searchKeyword$
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (keyword: string) => {
              keyword = keyword.trim();
              if (!keyword)
                this.router.navigate(['/home']);
              this.books = [];
              // Emit the new search keyword to the searchKeyword$ Subject
              // whenever it changes in the SearchStateService
              this.searchKeyword$.next(keyword);
            },
            error: (error: any) => {
              this.toastService.showSimpleError(error.toString());
            },
            complete: () => console.info('complete')
          });

        // Use the startWith operator to trigger the initial search
        // when the component is initialized with a search keyword
        this.searchKeyword$
          .pipe(
            startWith(this.keyword), // Use an initial empty string to trigger the search on component load
            debounceTime(500), // Wait for 500ms between consecutive requests
            distinctUntilChanged(), // Ignore consecutive identical requests
            takeUntil(this.destroy$) // Unsubscribe from the observable when the component is destroyed
          )
          .subscribe({
            next: (keyword: string) => {
              this.onSearch(keyword);
            },
            error: (error: any) => {
              this.toastService.showSimpleError(error.toString());
            },
            complete: () => console.info('complete')
          });
      },
      error: (error: any) => {
        this.toastService.showSimpleError(error.toString());
      },
      complete: () => console.info('complete')
    });
  }

  onSearch(keyword: string): void {
    this.searchService
      .getSearchResults(keyword)?.pipe(
        takeUntil(this.destroy$),
        catchError((error) => {
          this.toastService.showError(error as ErrorResponse);
          return of(null); // Return a new observable with null value to continue the observable chain
        })
      )
      .subscribe({
        next: (response: any) => {
          if (!response || response?.books?.length === 0) {
            this.books = undefined;
            return;
          }
          this.books = response?.books?.map(
            (book: BookResponseDto) => {
              return new BookResponseDto(book);
            }
          );
        },
        error: (error: any) => {
          this.toastService.showSimpleError(error.toString());
        },
        complete: () => console.info('complete')
      });
  }

}
