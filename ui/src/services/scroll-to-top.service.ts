import { Injectable } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs";

@Injectable()
export class ScrollToTopService {
    constructor(router: Router) {
        router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe({
            next: (r) => {
                if (r instanceof NavigationEnd) {
                    if (!r.url.includes("search")) {
                        const targetElement = document.querySelector('#targetRootOutlet');
                        if (targetElement) {
                            targetElement.scrollIntoView({ behavior: 'smooth' });
                        }
                    } else {
                        const targetSearch = document.querySelector('#targetSearch');
                        if (targetSearch) {
                            targetSearch.scrollIntoView({ behavior: 'smooth' });
                        }
                    }
                }
                //window.scrollTo(0, 0);
            }
        });
    }
}