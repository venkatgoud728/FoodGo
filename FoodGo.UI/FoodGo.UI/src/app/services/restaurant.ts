import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {

  private apiUrl = 'https://localhost:7006/api/Restaurants';

  constructor(private http: HttpClient) {}

  getRestaurants(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}