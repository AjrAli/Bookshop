import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { StepsModule } from 'primeng/steps';
@Component({
  selector: 'app-steps',
  standalone: true,
  imports: [StepsModule],
  templateUrl: './steps.component.html',
  styleUrl: './steps.component.css'
})
export class StepsComponent implements OnInit {
  items!: MenuItem[];

  activeIndex: number = 0;

  ngOnInit() {
    this.items = [
      {
        label: 'Shopping Cart',
        routerLink: 'my-shoppingcart',
      },
      {
        label: 'Authentication',
        routerLink: 'login',
      },
      {
        label: 'Payment',
        routerLink: 'payment',
      },
    ];
  }
}
