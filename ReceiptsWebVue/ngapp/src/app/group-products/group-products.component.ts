import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductsService } from '../products.service';

@Component({
  selector: 'app-group-products',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './group-products.component.html',
  styleUrl: './group-products.component.css'
})

export class GroupProductsComponent {
  loading: boolean = false;

  filterGroup:string= '';
  filterGroupValues:string[] = [];
  searchString:string = '';
  sort:string = 'Group';
  products1price:boolean = false;
  pageSize:number = 10;
  pageNumber:number = 1;

  results: any[] = [];
  pageIndex: number= 1
  totalPages: number = 1;
  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;

  constructor(private productsService: ProductsService) { }

  ngOnInit() {
    this.productsService.getGroupSelectList().subscribe((data: any) => {
      this.filterGroupValues = data;
    });
  }

  init() {
    this.filterGroup = '';
    this.searchString = '';
    this.sort = 'Group';
    this.products1price = false;
    this.pageSize = 10;
    this.pageNumber = 1;
  }

  submitChanges(): void {
    this.loading = true;

    this.productsService.getGroupProducts(this.filterGroup, this.searchString, this.sort, this.products1price, this.pageSize, this.pageNumber)
      .subscribe((data: any) => {
        this.pageIndex = data.pageIndex;
        this.totalPages = data.totalPages;
        this.results = data.data;
        this.hasPreviousPage = data.hasPreviousPage;
        this.hasNextPage = data.hasNextPage;
        this.loading = false;
    });
  }

  formatDate(date:string) {
    var options:Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: '2-digit',
      day: 'numeric'
    };
    var d = new Date(date.slice(0, 10))
    return d.toLocaleString(navigator.language ? navigator.language : navigator['language'], options)
  }

  changePageSize() {
    this.pageNumber = 1
    this.submitChanges()
  }
}
