import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from './services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  email = '';
  password = '';

  errorMessage = '';
  successMessage = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.email || !this.password) {
      this.errorMessage = 'Please enter email and password.';
      return;
    }

    this.loading = true;

    this.authService
      .login(this.email, this.password)
      .subscribe({
        next: (response: any) => {

          console.log('Login successful:', response);

          this.loading = false;
          this.successMessage = 'Login successful!';

          // Store the logged-in user
          localStorage.setItem(
            'foodgoUser',
            JSON.stringify(response)
          );

          // Go back to Home and reload the app
          this.router.navigate(['/']).then(() => {
            window.location.reload();
          });

        },

        error: (error: any) => {

          console.error('Login error:', error);

          this.loading = false;

          if (error.status === 401) {
            this.errorMessage = 'Invalid email or password.';
          } else {
            this.errorMessage =
              'Login failed. Please try again.';
          }

        }
      });
  }
}