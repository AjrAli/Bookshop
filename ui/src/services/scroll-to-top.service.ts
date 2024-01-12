import { Injectable } from "@angular/core";
import { Router, NavigationEnd } from "@angular/router";
import { filter } from "rxjs";

@Injectable()
export class ScrollToTopService {
    constructor(router: Router) {
        router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe({
            next: (r) => {
                //window.scrollTo(0, 0);
                const targetElement = document.querySelector('#targetRootOutlet');
                if (targetElement) {
                    targetElement.scrollIntoView({ behavior: 'smooth' });
                }
            }
        });
    }
}