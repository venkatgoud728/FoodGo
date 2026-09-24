import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

import { RestaurantService } from './services/restaurant';
import { FoodService } from './services/food';
import { CartService } from './services/cart';
import { AuthService } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  restaurants: any[] = [];

  foodItems: any[] = [];

  selectedRestaurant: any = null;

  cartItemCount = 0;
  isLoggedIn = false;
loggedInUser: any = null;

  constructor(
    private restaurantService: RestaurantService,
    private foodService: FoodService,
    private cartService: CartService,
     private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

  const storedUser = localStorage.getItem('foodgoUser');

  if (storedUser) {
    this.loggedInUser = JSON.parse(storedUser);
    this.isLoggedIn = true;
  }

    // Load restaurants
    this.restaurantService.getRestaurants().subscribe({

      next: (data) => {

        console.log('Restaurants received:', data);

        this.restaurants = data;

        this.cdr.detectChanges();

      },

      error: (error) => {

        console.error(
          'Error loading restaurants:',
          error
        );

      }

    });

    // Load cart count when application starts
    this.loadCartCount();

    // Refresh cart count whenever cart changes
    this.cartService.cartUpdated$.subscribe(() => {

      this.loadCartCount();

    });

  }

  loadCartCount(): void {

  const storedUser =
    localStorage.getItem('foodgoUser');

  if (!storedUser) {
    this.cartItemCount = 0;
    return;
  }

  const user = JSON.parse(storedUser);

  this.cartService
    .getUserCart(user.userId)
    .subscribe({

      next: (data: any) => {

        console.log(
          'Cart count data:',
          data
        );

        if (data && data.items) {

          this.cartItemCount =
            data.items.reduce(
              (total: number, item: any) =>
                total + item.quantity,
              0
            );

        } else {

          this.cartItemCount = 0;

        }

        this.cdr.detectChanges();

      },

      error: (error: any) => {

        console.error(
          'Error loading cart count:',
          error
        );

        this.cartItemCount = 0;

      }

    });

}

  // Load food items for selected restaurant
  loadFoodItems(restaurant: any): void {

    this.selectedRestaurant = restaurant;

    console.log(
      'Selected restaurant:',
      restaurant
    );

    this.foodService
      .getFoodItemsByRestaurant(
        restaurant.restaurantId
      )
      .subscribe({

        next: (data) => {

          console.log(
            'Food items received:',
            data
          );

          this.foodItems = data;

          this.cdr.detectChanges();

        },

        error: (error) => {

          console.error(
            'Error loading food items:',
            error
          );

        }

      });

  }

  // Add food item to cart
  addToCart(food: any): void {

    const cartId = 1;
    const quantity = 1;

    console.log(
      'Sending cart request:',
      {
        cartId: cartId,
        foodItemId: food.foodItemId,
        quantity: quantity
      }
    );

    this.cartService
      .addToCart(
        cartId,
        food.foodItemId,
        quantity
      )
      .subscribe({

        next: (data: any) => {

          console.log(
            'Added to cart:',
            data
          );

          // Tell the application that the cart changed
          this.cartService.notifyCartUpdated();

          alert(
            `${food.name} added to cart!`
          );

        },
        

        error: (error: any) => {

          console.error(
            'FULL CART ERROR:',
            error
          );

          console.error(
            'Status:',
            error.status
          );

          console.error(
            'Backend response:',
            error.error
          );

          alert(
            'Add to cart failed: ' +
            (error.error || 'Unknown error')
          );

        }

      });

  }

  logout(): void {
  localStorage.removeItem('foodgoUser');

  this.loggedInUser = null;
  this.isLoggedIn = false;
}

}