import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  url = 'https://localhost:7136';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<any> {
    return this.http.get(this.url + '/Products', { headers: { Accept: 'application/json' } });    
  }

  getGroupProducts(): Observable<any> {
    return this.http.get(this.url + '/GroupProducts', { headers: { Accept: 'application/json' } });
  }

  getGroupSelectList(): Observable<any> {    
    return this.http.get(this.url + '/GroupSelectList', { headers: { Accept: 'application/json' } });
  }
}
