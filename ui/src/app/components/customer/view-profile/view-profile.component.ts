import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CustomerResponseDto } from '../../../dto/customer/customer-response-dto';
import { CustomerDataService } from '../../../../services/customer/customer-data.service';

@Component({
  selector: 'app-view-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './view-profile.component.html',
  styleUrl: './view-profile.component.css'
})
export class ViewProfileComponent implements OnInit {

  customer: CustomerResponseDto | null = null;
  constructor(private customerDataService: CustomerDataService) { }
  ngOnInit(): void {
    this.customer = this.customerDataService.getCustomer();
  }

}
