import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderResponseDto } from '../../dto/order/order-response-dto';
import { ButtonModule } from 'primeng/button';
import { OrderService } from '../../../services/order.service';
import { OrderApiService } from '../../../services/order/order-api.service';
import { PaginatorModule } from 'primeng/paginator';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { PageEvent } from '../../body/list-cards/page-event';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, ButtonModule, PaginatorModule, ProgressSpinnerModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit, OnDestroy {
  orders: OrderResponseDto[] | undefined;
  totalRecords: number = 0;
  rows: number = 10; // Number of items per page
  first: number = 0; // Initial page index
  private orderSubscription: Subscription | undefined;
  private orderUpdateSubscription: Subscription | undefined;
  constructor(private OrderService: OrderService, private orderApiService: OrderApiService, private router: Router) { }
  ngOnDestroy(): void {
    this.orderSubscription?.unsubscribe();
    this.orderUpdateSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.orders = [];
    this.orderSubscription = this.orderApiService.getOrders().subscribe({
      next: (r) => {
        if (r && r.orders?.length > 0) {
          this.orders = r.orders.map((order: OrderResponseDto) => {
            return new OrderResponseDto(order);
          });
          this.totalRecords = this.orders.length;
        } else {
          this.orders = undefined;
        }
      },
      error: (e) => {
        this.orders = undefined;
      }
    });
  }
  onPageChange(event: any): void {
    let eventOfPageEvent = event as PageEvent;
    if (eventOfPageEvent) {
      this.first = eventOfPageEvent.first;
      this.rows = eventOfPageEvent.rows;
    }
  }
  navigateToOrder(id: number) {
    if (id)
      this.router.navigate([`/order/${id}`]);
  }
  cancelOrder(id: number) {
    if (id > 0) {
      this.orderUpdateSubscription = this.OrderService.cancelOrderFromApi(id).subscribe({
        next: (r) => {
          if (r) {
            if (this.orders && this.orders.length > 0) {
              const orderIndexToDelete = this.orders.findIndex(x => x.id === id);
              this.orders.splice(orderIndexToDelete, 1)
              this.totalRecords = this.orders.length;
              if (this.orders.length === 0)
                this.orders = undefined;
            }
          }
        }
      })
    }
  }
}
