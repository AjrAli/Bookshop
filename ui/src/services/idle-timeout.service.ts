import { Injectable, OnDestroy } from '@angular/core';
import { Subject, Observable, fromEvent, debounceTime, merge, Subscription, BehaviorSubject } from 'rxjs';

@Injectable()
export class IdleTimeoutService implements OnDestroy {
  private idleTimer: any;
  private readonly DEFAULT_TIMER = 3600000;
  private idleTimeoutDuration = this.DEFAULT_TIMER; // Default to 60 minutes
  private idleTimeoutSubject: BehaviorSubject<Event | null>;
  private userActivitySubscription: Subscription | undefined;

  constructor() {
    this.idleTimeoutSubject = new BehaviorSubject<Event | null>(null);
  }

  resetIdleTimer(): void {
    clearTimeout(this.idleTimer);
    this.idleTimer = setTimeout(() => { }, this.idleTimeoutDuration);
  }

  onIdleTimeout(): Observable<Event | null> {
    this.resetIdleTimer(); // Make sure to reset the timer
    const mouseMove$ = fromEvent(document, 'mousemove').pipe(debounceTime(500));
    const click$ = fromEvent(document, 'click').pipe(debounceTime(500));
    const keyDown$ = fromEvent(document, 'keydown').pipe(debounceTime(500));
    this.userActivitySubscription = merge(click$, mouseMove$, keyDown$).subscribe({
      next: (r) => {
        if (r)
          this.idleTimeoutSubject.next(r);
      },
      error: (e) => {
      },
      complete: () => {
        this.idleTimeoutSubject.complete();
      }
    });
    return this.idleTimeoutSubject.asObservable();
  }

  setIdleTimeoutDuration(duration: number): void {
    this.idleTimeoutDuration = duration;
    this.resetIdleTimer();
  }

  stopIdleTimer(): void {
    // Stop the idle timer and perform any necessary cleanup
    clearTimeout(this.idleTimer);
    this.userActivitySubscription?.unsubscribe();
    this.idleTimeoutSubject.complete(); // Complete the subject when stopping the timer
    this.idleTimeoutSubject = new BehaviorSubject<Event | null>(null); // Create a new subject
  }
  ngOnDestroy(): void {
    // Ensure to stop the idle timer when the service is destroyed
    this.stopIdleTimer();
  }
}
