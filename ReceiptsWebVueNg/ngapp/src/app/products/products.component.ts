import { Component } from '@angular/core';
import { ProductsService } from '../services/products.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {
  results: string = '';
  pageIndex: number= 1
  totalPages: number = 1;
  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;

  constructor(private productsService: ProductsService) { }

    fetchProducts(): void {
      this.productsService.getProducts().subscribe((data: any) => {
        this.pageIndex = data.pageIndex;
        this.totalPages = data.totalPages;
        this.results = data.data;
        this.hasPreviousPage = data.hasPreviousPage;
        this.hasNextPage = data.hasNextPage;
      });
    }
}
