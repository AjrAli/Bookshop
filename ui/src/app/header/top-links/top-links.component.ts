import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { CustomerDataService } from '../../../services/customer/customer-data.service';
import { ButtonModule } from 'primeng/button';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-top-links',
  standalone: true,
  imports: [CommonModule, ButtonModule, TieredMenuModule],
  templateUrl: './top-links.component.html',
  styleUrl: './top-links.component.css'
})
export class TopLinksComponent implements OnInit, OnDestroy {
  customerName: string | undefined;
  items: MenuItem[] | undefined;
  connected: boolean = false;
  private customerSubscription: Subscription | undefined;
  constructor(private customerService: CustomerService,
    private customerDataService: CustomerDataService,
    private router: Router) { }

  ngOnDestroy(): void {
    this.customerSubscription?.unsubscribe();
  }
  ngOnInit() {
    this.customerSubscription = this.customerDataService.getCustomerObservable().subscribe({
      next: (customer) => {
        if (customer) {
          this.connected = true;
          this.customerName = customer.firstName;
          this.items = [
            {
              label: 'Orders',
              icon: 'pi pi-shopping-bag',
              command: () => this.router.navigate(['/orders'])
            },
            {
              label: 'Profile',
              icon: 'pi pi-user',
              items: [
                {
                  label: 'View',
                  icon: '',
                  command: () => this.router.navigate(['/customer/view-profile'])
                },
                {
                  label: 'Edit password',
                  icon: '',
                  command: () => this.router.navigate(['/customer/edit-password'])
                },
                {
                  label: 'Edit profile',
                  icon: '',
                  command: () => this.router.navigate(['/customer/edit-profile'])
                }
              ]
            }
          ]
        } else {
          this.connected = false;
          this.customerName = undefined;
          this.items = [];
        }
      }
    });

  }
  navigateToLogin() {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/login']);
    });
  }
  logout(): void {
    this.customerService.logout();
    this.router.navigate(['']);
  }
}
