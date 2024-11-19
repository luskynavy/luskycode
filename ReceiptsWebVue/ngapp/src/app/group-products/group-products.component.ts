import { Component } from '@angular/core';
import { ProductsService } from '../products.service';

@Component({
  selector: 'app-group-products',
  standalone: true,
  imports: [],
  templateUrl: './group-products.component.html',
  styleUrl: './group-products.component.css'
})

/*class GroupProducts1 {
  id:number = 0;
  name:string = '';
}*/

export class GroupProductsComponent {  
  results: any[] = [];  
  pageIndex: number= 1
  totalPages: number = 1;
  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;

  constructor(private productsService: ProductsService) { }  

  fetchGroupProducts(): void {
    this.productsService.getGroupProducts().subscribe((data: any) => {
      this.pageIndex = data.pageIndex;
      this.totalPages = data.totalPages;
      this.results = data.data;
      this.hasPreviousPage = data.hasPreviousPage;
      this.hasNextPage = data.hasNextPage;
    });
  }

  formatDate(date:string) {
    return date;
  }

}
