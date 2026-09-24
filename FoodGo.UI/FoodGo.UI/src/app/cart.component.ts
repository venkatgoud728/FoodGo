import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartService } from './services/cart';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {

  cart: any = null;

  loading = false;

  errorMessage = '';

  // Temporary user ID for testing
  userId = 0;

  constructor(
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

  this.loadCart();
}

  // Load cart from backend
  loadCart(): void {

    this.loading = true;
    this.errorMessage = '';

    this.cartService
      .getUserCart(this.userId)
      .subscribe({

        next: (data: any) => {

          console.log(
            'Cart received:',
            data
          );

          this.cart = data;

          this.loading = false;

          // Refresh cart screen
          this.cdr.detectChanges();

        },

        error: (error: any) => {

          console.error(
            'Error loading cart:',
            error
          );

          this.loading = false;

          if (error.status === 404) {

            this.cart = {
              items: [],
              total: 0
            };

          } else {

            this.errorMessage =
              'Unable to load cart.';

          }

          this.cdr.detectChanges();

        }

      });
  }

  // Increase quantity
  increaseQuantity(item: any): void {

    const newQuantity =
      item.quantity + 1;

    this.cartService
      .updateCartItem(
        item.cartItemId,
        newQuantity
      )
      .subscribe({

        next: () => {

          this.loadCart();

          this.cartService.notifyCartUpdated();

        },

        error: (error: any) => {

          console.error(
            'Error increasing quantity:',
            error
          );

        }

      });
  }

  // Decrease quantity
  decreaseQuantity(item: any): void {

    if (item.quantity <= 1) {

      this.removeItem(item);

      return;

    }

    const newQuantity =
      item.quantity - 1;

    this.cartService
      .updateCartItem(
        item.cartItemId,
        newQuantity
      )
      .subscribe({

        next: () => {

          this.loadCart();

          this.cartService.notifyCartUpdated();

        },

        error: (error: any) => {

          console.error(
            'Error decreasing quantity:',
            error
          );

        }

      });
  }

 removeItem(item: any): void {

  this.cartService
    .removeCartItem(item.cartItemId)
    .subscribe({

      next: () => {

        console.log('Item removed from cart');

        // Refresh cart UI immediately
        this.loadCart();

        // Notify navbar cart count
        this.cartService.notifyCartUpdated();

      },

      error: (error: any) => {

        console.error(
          'Error removing item:',
          error
        );

      }

    });

}
}