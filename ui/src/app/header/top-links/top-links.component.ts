import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';

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
    private router: Router) { }

  isConnected() {
    if (this.customerService.isLoggedIn()) {
      this.customerName = this.customerService.getCustomerInfo()?.firstName;
      return true;
    }
    return false;
  }
  logout(): void {
    this.customerService.logout();
    this.router.navigate(['']);
  }
}
