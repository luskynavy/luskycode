import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  url = 'https://localhost:7136';

  mockHttpGet = false;

  constructor(private http: HttpClient) {}

  getProducts(filterGroup:string, searchString:string | null, sort:string, pageSize:number, pageNumber:number): Observable<any> {
    if (this.mockHttpGet) {
      return of({
      "pageIndex": 1,
      "totalPages": 126,
      "data": [
        {
          "id": 3778,
          "name": "BANANE",
          "group": "FRUITS",
          "price": 4.99,
          "dateReceipt": "2024-07-29T00:00:00",
          "sourceName": "Ticket de caisse_29072024-130556.pdf",
          "sourceLine": 34,
          "fullData": "TOMATE RONDE 0,818 kg  x     4,99 €/kg         4,08 €"
        },
        {
          "id": 4090,
          "name": "ABRICOT",
          "group": "FRUITS",
          "price": 3.69,
          "dateReceipt": "2024-12-06T00:00:00",
          "sourceName": "Ticket de caisse_06122024-134548.pdf",
          "sourceLine": 44,
          "fullData": "TOMATE 1,188 kg  x     3,69 €/kg         4,38 €"
        }
        ],
        "hasPreviousPage": false,
        "hasNextPage": true
      })
    } else {
      const httpParams  = new HttpParams()
        .set('filterGroup', filterGroup)
        .set('searchString', searchString != null ? searchString : '')
        .set('sort', sort)
        .set('pageSize', pageSize)
        .set('pageNumber', pageNumber)

      return this.http.get(this.url + '/Products',
      {
        headers: { Accept: 'application/json' },
        params: httpParams
      });
    }
  }

  getGroupProducts(filterGroup:string, searchString:string, sort:string, products1price:boolean, pageSize:number, pageNumber:number): Observable<any> {
    if (this.mockHttpGet) {
    return of({
        "pageIndex": 1,
        "totalPages": 11,
        "data": [
          {
            "id": 4090,
            "name": "BANANE",
            "group": "FRUITS",
            "min": 3.69,
            "max": 4.99,
            "minDate": "2024-07-29T00:00:00",
            "maxDate": "2024-12-06T00:00:00",
            "priceRatio": 1.3523035230352303,
            "pricesCount": 2
          },
          {
            "id": 3736,
            "name": "ABRICOT",
            "group": "FRUITS",
            "min": 3.49,
            "max": 4.99,
            "minDate": "2021-07-02T00:00:00",
            "maxDate": "2024-08-02T00:00:00",
            "priceRatio": 1.4297994269340975,
            "pricesCount": 2
          },
          {
            "id": 2304,
            "name": "AVOCAT HASS AFFINE",
            "group": "FRUITS",
            "min": 1.49,
            "max": 1.69,
            "minDate": "2021-05-21T00:00:00",
            "maxDate": "2023-02-11T00:00:00",
            "priceRatio": 1.1342281879194631,
            "pricesCount": 2
          }
        ],
        "hasPreviousPage": false,
        "hasNextPage": true
      })
    } else {
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
  }

  getGroupSelectList(): Observable<any> {
    if (this.mockHttpGet) {
      return of(['FRUITS', 'LEGUMES'])
    } else {
      return this.http.get(this.url + '/GroupSelectList', { headers: { Accept: 'application/json' } });
    }
  }

  getProductsNames(group:string): Observable<any> {
    if (this.mockHttpGet) {
      return of(['BANANES', 'ABRICOT', 'TOMATE RONDE'])
    } else {
      const httpParams  = new HttpParams()
        .set('group', group)
      return this.http.get(this.url + '/ProductsNames',
      {
        headers: { Accept: 'application/json' },
        params: httpParams
      });
    }
  }

  getProduct(id:number): Observable<any> {
    if (this.mockHttpGet) {
      return of({
        "id": 4000,
        "name": "FROM.PAST.28%MG COUSTERON 320G",
        "group": "FROMAGE LS",
        "price": 3.27,
        "dateReceipt": "2024-10-26T00:00:00",
        "sourceName": "..\\..\\..\\Tickets\\Ticket de caisse_26102024-112544.pdf",
        "sourceLine": 23,
        "fullData": "FROM.PAST.28%MG COUSTERON 320G   (T)        3,27 €  11"
          })
      } else {
        return this.http.get(this.url + '/Products/'+id,
        {
          headers: { Accept: 'application/json' }
        });
      }
  }

  getProductPrices(id:number): Observable<any> {
    if (this.mockHttpGet) {
      return of ([
        {
          "price": 2.9,
          "dateReceipt": "2022-02-12T00:00:00"
        },
        {
          "price": 3.13,
          "dateReceipt": "2023-10-12T00:00:00"
        },
        {
          "price": 3.26,
          "dateReceipt": "2023-12-15T00:00:00"
        },
        {
          "price": 3.27,
          "dateReceipt": "2024-10-26T00:00:00"
        }
      ])
    } else {
      const httpParams  = new HttpParams()
        .set('id', id)

      return this.http.get(this.url + '/GetProductPrices',
      {
        headers: { Accept: 'application/json' },
        params: httpParams
      });
    }
  }

  exportGroupProductsMiniExcel(): Observable<any> {
    if (this.mockHttpGet) {
      const fakeFile = new Blob(['fake file'], {
        type: "text/html",
      });
      return of(fakeFile)
    } else {
      return this.http.get(this.url + '/ExportGroupProductsMiniExcel', { responseType: 'blob' });
    }
  }

  exportProductsMiniExcel(): Observable<any> {
    if (this.mockHttpGet) {
      const fakeFile = new Blob(['fake file'], {
        type: "text/html",
      });
      return of(fakeFile)
    } else {
      return this.http.get(this.url + '/ExportProductsMiniExcel', { responseType: 'blob' });
    }
  }
}
