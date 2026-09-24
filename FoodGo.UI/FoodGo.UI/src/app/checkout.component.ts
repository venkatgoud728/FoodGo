import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AddressService } from './services/address';
import { CartService } from './services/cart';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {

  userId = 0;

  addresses: any[] = [];
  selectedAddressId: number | null = null;

  cart: any = null;

  loading = false;
  placingOrder = false;

  errorMessage = '';
  successMessage = '';

  constructor(
    private addressService: AddressService,
    private cartService: CartService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    const storedUser = localStorage.getItem('foodgoUser');

    if (!storedUser) {
      this.router.navigate(['/login']);
      return;
    }

    const user = JSON.parse(storedUser);

    this.userId = user.userId;

    this.loadAddresses();
    this.loadCart();
  }

  loadAddresses(): void {

    this.loading = true;
    this.errorMessage = '';

    this.addressService
      .getAddresses()
      .subscribe({
        next: (data: any[]) => {

          this.addresses = data.filter(
            address => address.userId === this.userId
          );

          if (this.addresses.length > 0) {
            this.selectedAddressId =
              this.addresses[0].addressId;
          }

          this.loading = false;
          this.cdr.detectChanges();
        },

        error: (error: any) => {

          console.error(
            'Error loading addresses:',
            error
          );

          this.loading = false;
          this.errorMessage =
            'Unable to load addresses.';
        }
      });
  }

  loadCart(): void {

    this.cartService
      .getUserCart(this.userId)
      .subscribe({
        next: (data: any) => {

          this.cart = data;

          this.cdr.detectChanges();
        },

        error: (error: any) => {

          console.error(
            'Error loading cart:',
            error
          );

          this.errorMessage =
            'Unable to load cart.';
        }
      });
  }

  placeOrder(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedAddressId) {
      this.errorMessage =
        'Please select an address.';
      return;
    }

    if (!this.cart || !this.cart.items?.length) {
      this.errorMessage =
        'Your cart is empty.';
      return;
    }

    this.placingOrder = true;

    const url =
      `https://localhost:7006/api/Orders` +
      `?userId=${this.userId}` +
      `&addressId=${this.selectedAddressId}`;

    fetch(url, {
      method: 'POST'
    })
      .then(async response => {

        if (!response.ok) {
          const errorText = await response.text();
          throw new Error(errorText);
        }

        return response.json();
      })
      .then(order => {

        console.log(
          'Order placed:',
          order
        );

        this.placingOrder = false;

        this.successMessage =
          `Order #${order.orderId} placed successfully!`;

        this.cartService.notifyCartUpdated();

        setTimeout(() => {
          this.router.navigate(['/orders']);
        }, 1000);
      })
      .catch(error => {

        console.error(
          'Error placing order:',
          error
        );

        this.placingOrder = false;

        this.errorMessage =
          error.message ||
          'Unable to place order.';
      });
  }
}