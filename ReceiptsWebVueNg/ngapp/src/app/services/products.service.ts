import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
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

  getGroupProducts(filterGroup:string, searchString:string, sort:string, products1price:boolean, pageSize:number, pageNumber:number): Observable<any> {
    const httpParams  = new HttpParams()
      .set('filterGroup', filterGroup)
      .set('searchString', searchString)
      .set('sort', sort)
      .set('products1price', products1price)
      .set('pageSize', pageSize)
      .set('pageNumber', pageNumber)

    return this.http.get(this.url + '/GroupProducts',
    {
      headers: { Accept: 'application/json' },
      params: httpParams
    });
  }

  getGroupSelectList(): Observable<any> {
    return this.http.get(this.url + '/GroupSelectList', { headers: { Accept: 'application/json' } });
  }

  getProductsNames(group:string): Observable<any> {
    const httpParams  = new HttpParams()
      .set('group', group)
    return this.http.get(this.url + '/ProductsNames',
    {
      headers: { Accept: 'application/json' },
      params: httpParams
    });
  }

  getProduct(id:number): Observable<any> {

    return this.http.get(this.url + '/Products/'+id,
    {
      headers: { Accept: 'application/json' }
    });
  }

  getProductPrices(id:number): Observable<any> {
    const httpParams  = new HttpParams()
      .set('id', id)

    return this.http.get(this.url + '/GetProductPrices',
    {
      headers: { Accept: 'application/json' },
      params: httpParams
    });
  }
}
