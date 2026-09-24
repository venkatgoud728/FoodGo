import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private apiUrl = 'https://localhost:7006/api/CartItems';

  // Used to notify other components when cart changes
  private cartUpdatedSource = new Subject<void>();

  cartUpdated$ = this.cartUpdatedSource.asObservable();

  constructor(private http: HttpClient) {}

  // Notify that cart has changed
  notifyCartUpdated(): void {
    this.cartUpdatedSource.next();
  }

  // Add food item to cart
  addToCart(
    cartId: number,
    foodItemId: number,
    quantity: number
  ): Observable<any> {

    return this.http.post<any>(
      this.apiUrl,
      {
        cartId: cartId,
        foodItemId: foodItemId,
        quantity: quantity
      }
    );
  }

  // Get user's cart
  getUserCart(userId: number): Observable<any> {

    return this.http.get<any>(
      `${this.apiUrl}/user/${userId}`
    );
  }

  // Update cart item quantity
  updateCartItem(
    cartItemId: number,
    quantity: number
  ): Observable<any> {

    return this.http.put(
      `${this.apiUrl}/${cartItemId}`,
      {
        cartItemId: cartItemId,
        quantity: quantity
      }
    );
  }

  // Remove cart item
  removeCartItem(cartItemId: number): Observable<any> {

    return this.http.delete(
      `${this.apiUrl}/${cartItemId}`
    );
  }

}