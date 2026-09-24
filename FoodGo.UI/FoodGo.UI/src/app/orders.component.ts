import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CartService } from './services/cart';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {

  orders: any[] = [];

  loading = false;
  errorMessage = '';

  userId = 0;

  private apiUrl = 'https://localhost:7006/api/Orders';

  constructor(
    private http: HttpClient,
     private cartService: CartService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    const storedUser =
      localStorage.getItem('foodgoUser');

    if (!storedUser) {
      return;
    }

    const user = JSON.parse(storedUser);

    this.userId = user.userId;

    this.loadOrders();
    this.cartService.notifyCartUpdated();
  }

  loadOrders(): void {

    this.loading = true;
    this.errorMessage = '';

    this.http
      .get<any[]>(
        `${this.apiUrl}/user/${this.userId}`
      )
      .subscribe({

        next: (data) => {

          console.log(
            'Orders received:',
            data
          );

          this.orders = data;

          this.loading = false;

          this.cdr.detectChanges();
        },

        error: (error) => {

          console.error(
            'Error loading orders:',
            error
          );

          this.loading = false;

          this.errorMessage =
            'Unable to load orders.';

          this.cdr.detectChanges();
        }

      });
  }

}