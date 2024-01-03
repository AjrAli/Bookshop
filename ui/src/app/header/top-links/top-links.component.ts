import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { CustomerDataService } from '../../../services/customer/customer-data.service';

@Component({
  selector: 'app-top-links',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './top-links.component.html',
  styleUrl: './top-links.component.css'
})
export class TopLinksComponent {
  customerName: string | undefined;
  constructor(private customerService: CustomerService,
    private customerDataService: CustomerDataService,
    private router: Router) { }

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
