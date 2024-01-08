import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { CustomerDataService } from '../../../services/customer/customer-data.service';
import { ButtonModule } from 'primeng/button';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-top-links',
  standalone: true,
  imports: [CommonModule, RouterModule, ButtonModule, TieredMenuModule],
  templateUrl: './top-links.component.html',
  styleUrl: './top-links.component.css'
})
export class TopLinksComponent implements OnInit {
  customerName: string | undefined;
  items: MenuItem[] | undefined;
  constructor(private customerService: CustomerService,
    private customerDataService: CustomerDataService,
    private router: Router) { }
  ngOnInit() {
    this.items = [
      {
        label: 'Orders',
        icon: 'pi pi-shopping-bag'
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
  }
  isConnected() {
    if (this.customerService.isLoggedIn()) {
      this.customerName = this.customerDataService.getCustomer()?.firstName;
      return true;
    }
    return false;
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
