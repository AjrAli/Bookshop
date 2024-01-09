import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { OrderResponseDto } from '../../dto/order/order-response-dto';
import { ButtonModule } from 'primeng/button';
import { OrderService } from '../../../services/order.service';
import { OrderApiService } from '../../../services/order/order-api.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  orders: OrderResponseDto[] | null = null;

  constructor(private OrderService: OrderService, private orderApiService: OrderApiService) { }

  ngOnInit(): void {
    this.orderApiService.getOrders().subscribe({
      next: (r) => {
        if (r && r.orders)
          this.orders = r.orders.map((order: OrderResponseDto) => {
            return new OrderResponseDto(order);
          })
      }
    })
  }
  cancelOrder(id: number) {
    if (id > 0) {
      this.OrderService.cancelOrderFromApi(id).subscribe({
        next: (r) => {
          if (r) {
            const orderIndexToDelete = this.orders?.findIndex(x => x.id === id);
            if (orderIndexToDelete)
              this.orders?.splice(orderIndexToDelete, 1)
          }
        }
      })
    }
  }
}
