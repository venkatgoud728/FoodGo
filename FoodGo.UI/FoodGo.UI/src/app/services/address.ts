import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private apiUrl = 'https://localhost:7006/api/Addresses';

  constructor(private http: HttpClient) {}

  getAddresses(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  addAddress(address: any): Observable<any> {
    return this.http.post<any>(
      this.apiUrl,
      address
    );
  }
}