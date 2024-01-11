import { Injectable, OnDestroy } from '@angular/core';
import { Observable, fromEvent, debounceTime, merge, Subscription, BehaviorSubject } from 'rxjs';

@Injectable()
export class IdleTimeoutService implements OnDestroy {
  private idleTimer: any;
  private readonly DEFAULT_TIMER = 3600000; // Default idle timer duration: 1 hour
  private idleTimeoutDuration = this.DEFAULT_TIMER; // Current idle timer duration
  private idleTimeoutSubject: BehaviorSubject<Event | null>;

  // Subscription to user activity events
  private userActivitySubscription: Subscription | undefined;

  constructor() {
    // Initialize the idleTimeoutSubject as a BehaviorSubject with null initial value
    this.idleTimeoutSubject = new BehaviorSubject<Event | null>(null);
  }

  // Reset the idle timer with the current duration
  resetIdleTimer(): void {
    clearTimeout(this.idleTimer);
    // Set a new timeout for the idle timer
    this.idleTimer = setTimeout(() => { }, this.idleTimeoutDuration);
  }

  // Observable for idle timeout events
  onIdleTimeout(): Observable<Event | null> {
    this.resetIdleTimer(); // Make sure to reset the timer

    // Observables for mouse move, click, and key down events with debounce
    const mouseMove$ = fromEvent(document, 'mousemove').pipe(debounceTime(500));
    const click$ = fromEvent(document, 'click').pipe(debounceTime(500));
    const keyDown$ = fromEvent(document, 'keydown').pipe(debounceTime(500));

    // Merge user activity events
    this.userActivitySubscription = merge(click$, mouseMove$, keyDown$).subscribe({
      next: (event) => {
        if (event) {
          // Notify subscribers with the user activity event
          this.idleTimeoutSubject.next(event);
        }
      },
      error: (error) => {
        // Handle errors if needed
      },
      complete: () => {
        // Complete the subject when the subscription is complete
        this.idleTimeoutSubject.complete();
      }
    });

    // Return the Observable to listen to idle timeout events
    return this.idleTimeoutSubject.asObservable();
  }

  // Set a new duration for the idle timer and reset it
  setIdleTimeoutDuration(duration: number): void {
    this.idleTimeoutDuration = duration;
    this.resetIdleTimer();
  }

  // Stop the idle timer and perform necessary cleanup
  stopIdleTimer(): void {
    clearTimeout(this.idleTimer);
    // Unsubscribe from user activity events
    this.userActivitySubscription?.unsubscribe();
    // Complete the subject and create a new one
    this.idleTimeoutSubject.complete();
    this.idleTimeoutSubject = new BehaviorSubject<Event | null>(null);
  }

  // Lifecycle hook to stop the idle timer when the service is destroyed
  ngOnDestroy(): void {
    this.stopIdleTimer();
  }
}
