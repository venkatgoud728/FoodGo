import { Routes } from '@angular/router';
import { CartComponent } from './cart.component';
import { Home } from './home/home';
import { LoginComponent } from './login.component';
import { RegisterComponent } from './register.component';
import { CheckoutComponent } from './checkout.component';
import { OrdersComponent } from './orders.component';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  pathMatch: 'full'
  },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [authGuard]

  },
  {
  path: 'checkout',
  component: CheckoutComponent,
  canActivate: [authGuard]
},
{
  path: 'orders',
  component: OrdersComponent,
  canActivate: [authGuard]
},
  
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  }
];