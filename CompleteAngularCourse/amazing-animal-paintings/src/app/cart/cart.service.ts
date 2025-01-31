import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private apiCatUrl = environment.apiUrl + "/cart";
  private apiCheckoutUrl = environment.apiUrl + "/checkout";

  constructor(private http: HttpClient) { }

  addToCart(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiCatUrl, product);
  }

  getCarItems(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiCatUrl);
  }

  cleanCart():  Observable<void> {
    return this.http.delete<void>(this.apiCatUrl);
  }

  checkout(products: Product[]):  Observable<void> {
    return this.http.post<void>(this.apiCheckoutUrl, products);
  }
}
