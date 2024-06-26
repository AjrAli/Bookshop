import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { ListShopItemsComponent } from '../../body/list-shop-items/list-shop-items.component';
import { OrderResponseDto } from '../../dto/order/order-response-dto';
import { OrderService } from '../../../services/order.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../services/toast.service';
import { OrderApiService } from '../../../services/order/order-api.service';
import { ErrorResponse } from '../../dto/response/error/error-response';
import { UpdateOrderDto } from '../../dto/order/update-order-dto';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [CommonModule, ButtonModule, ListShopItemsComponent, DividerModule],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.css'
})
export class OrderDetailsComponent implements OnInit, OnDestroy {

  @ViewChild('listItems') listItems: ListShopItemsComponent | undefined;
  @Input() order: OrderResponseDto | undefined;
  @Input() manage = "order";
  private orderSubscription: Subscription | undefined;
  private orderUpdateSubscription: Subscription | undefined;
  constructor(private route: ActivatedRoute,
    private router: Router,
    private orderApiService: OrderApiService,
    private orderService: OrderService,
    private toastService: ToastService) { }
  ngOnDestroy(): void {
    this.orderSubscription?.unsubscribe();
    this.orderUpdateSubscription?.unsubscribe();
  }
  ngOnInit(): void {
    if (!this.order) {
      const id = Number.parseInt(this.route.snapshot.params['id']);
      if (!Number.isNaN(id)) {
        this.orderSubscription = this.orderApiService.getOrderById(id).subscribe({
          next: (r) => {
            if (r.order) {
              this.order = new OrderResponseDto(r.order);
            }
          },
          error: (e: ErrorResponse) => {
            this.returnToListOrders();
            this.toastService.showError(e);
          }
        })
      } else {
        this.returnToListOrders();
      }
    }
  }
  removeItemsSelectedToApi() {
    const listIds = this.listItems?.shopIds;
    if (this.order && this.manage === 'order' && listIds && listIds.length > 0) {
      this.orderUpdateSubscription = this.orderService.updateOrderFromApi(new UpdateOrderDto({itemsId: listIds }), this.order.id).subscribe({
        next: (r) => {
          if (r) {
            this.listItems!.shopIds = [];
            this.order = new OrderResponseDto(r);
          } else {
            this.order = undefined;
            this.returnToListOrders();
          }
        }
      });
    }
  }
  returnToListOrders() {
    this.router.navigate(['/orders']);
  }

}
