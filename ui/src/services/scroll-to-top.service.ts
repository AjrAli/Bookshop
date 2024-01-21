import { Injectable, OnDestroy } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { Subscription, filter } from "rxjs";

@Injectable()
export class ScrollToTopService implements OnDestroy {
    private routerSubscription: Subscription | undefined;
    constructor(private router: Router) { }
    doScrollUpByNavigation() {
        this.routerSubscription = this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe({
            next: (r) => {
                if (r instanceof NavigationEnd) {
                    if (!r.url.includes("search")) {
                        this.scrollToRootOutlet();
                    } else {
                        this.scrollToSearch();
                    }
                }
            }
        });
    }
    ngOnDestroy(): void {
        this.routerSubscription?.unsubscribe();
    }

    scrollToRootOutlet() {
        this.scrollToContentById('#targetRootOutlet');
    }
    scrollToSearch() {
        this.scrollToContentById('#targetSearch');
    }
    scrollToBookItems() {
        this.scrollToContentById('#book-items');
    }
    scrollToOrderItems() {
        this.scrollToContentById('#order-items');
    }
    scrollToContentById(divId: string) {
        const targetElement = document.querySelector(divId);
        if (targetElement) {
            targetElement.scrollIntoView({ behavior: 'smooth' });
        }
    }
}