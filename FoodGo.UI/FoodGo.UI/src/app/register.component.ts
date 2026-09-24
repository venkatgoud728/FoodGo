import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from './services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  name = '';
  email = '';
  password = '';
  phone = '';

  errorMessage = '';
  successMessage = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  register(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.name || !this.email || !this.password) {
      this.errorMessage =
        'Name, email and password are required.';
      return;
    }

    this.loading = true;

    this.authService
      .register(
        this.name,
        this.email,
        this.password,
        this.phone
      )
      .subscribe({
        next: (response: any) => {
          console.log('Registration successful:', response);

          this.loading = false;
          this.successMessage =
            'Registration successful! Redirecting to login...';

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1000);
        },

        error: (error: any) => {
          console.error('Registration error:', error);

          this.loading = false;

          if (error.status === 400) {
            this.errorMessage =
              error.error || 'Email already exists.';
          } else {
            this.errorMessage =
              'Registration failed. Please try again.';
          }
        }
      });
  }
}