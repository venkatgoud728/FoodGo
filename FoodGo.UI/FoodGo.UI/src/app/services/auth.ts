import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7006/api/Auth';

  constructor(private http: HttpClient) {}

  register(
    name: string,
    email: string,
    password: string,
    phone: string
  ): Observable<any> {

    return this.http.post<any>(
      `${this.apiUrl}/register`,
      {
        name: name,
        email: email,
        password: password,
        phone: phone
      }
    );
  }

  login(
    email: string,
    password: string
  ): Observable<any> {

    return this.http.post<any>(
      `${this.apiUrl}/login`,
      {
        email: email,
        password: password
      }
    );
  }
}