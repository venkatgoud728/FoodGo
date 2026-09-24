import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';

import { RestaurantService } from '../services/restaurant';
import { FoodService } from '../services/food';
import { CartService } from '../services/cart';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,FormsModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {

 restaurants: any[] = [];
filteredRestaurants: any[] = [];
foodItems: any[] = [];
selectedRestaurant: any = null;

searchText = '';

  constructor(
    private restaurantService: RestaurantService,
    private foodService: FoodService,
    private cartService: CartService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    this.restaurantService.getRestaurants().subscribe({

      next: (data) => {

        console.log('Restaurants received:', data);

       this.restaurants = data;
this.filteredRestaurants = data;

this.cdr.detectChanges();

      },

      error: (error) => {

        console.error(
          'Error loading restaurants:',
          error
        );

      }

    });

  }




  searchRestaurants(): void {
  const search = this.searchText.trim().toLowerCase();

  if (!search) {
    this.filteredRestaurants = this.restaurants;
    return;
  }

  this.filteredRestaurants = this.restaurants.filter(
    restaurant =>
      restaurant.name?.toLowerCase().includes(search) ||
      restaurant.city?.toLowerCase().includes(search)
  );
}



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

  addToCart(food: any): void {

    // Get logged-in user
    const storedUser =
      localStorage.getItem('foodgoUser');

    if (!storedUser) {

      alert('Please login before adding items to your cart.');

      this.router.navigate(['/login']);

      return;
    }

    const user = JSON.parse(storedUser);

    const userId = user.userId;
    const quantity = 1;

    console.log(
      'Logged-in user:',
      user
    );

    // Get the logged-in user's cart
    this.cartService
      .getUserCart(userId)
      .subscribe({

        next: (cartData: any) => {

          const cartId = cartData.cartId;

          console.log(
            'Using cart:',
            cartId
          );

          console.log(
            'Sending cart request:',
            {
              cartId: cartId,
              foodItemId: food.foodItemId,
              quantity: quantity
            }
          );

          // Add food to the correct user's cart
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

        },

        error: (error: any) => {

          console.error(
            'Error loading user cart:',
            error
          );

          alert(
            'Unable to find your cart.'
          );

        }

      });

  }

}