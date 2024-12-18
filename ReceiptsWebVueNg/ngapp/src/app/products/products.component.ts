import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../services/products.service';
import { ProductPriceComponent } from '../product-price/product-price.component';
import { FormControl, FormsModule , ReactiveFormsModule} from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Observable } from 'rxjs/internal/Observable';
import { map, startWith } from 'rxjs/operators';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [FormsModule, RouterLink, ProductPriceComponent, MatInputModule, MatFormFieldModule, MatAutocompleteModule, ReactiveFormsModule, AsyncPipe],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {
  loading: boolean = false;

  filterGroup:string= '';
  filterGroupValues:string[] = [];
  searchString:string = '';
  productsNames:string[] = [];
  searchControl = new FormControl('');
  filteredProducts!: Observable<string[]>;
  sort:string = 'Group';
  pageSize:number = 10;
  pageNumber:number = 1;

  modalProductsPrices:boolean= false;
  modalProductName:string= '';
  modalProductId:number= 0;

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

    this.productsService.getProductsNames('').subscribe((data: any) => {
      this.productsNames = data;
    });

    this.filteredProducts = this.searchControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterProductsNames(value || '')),
    );
  }

  private _filterProductsNames(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.productsNames.filter(option => option.toLowerCase().includes(filterValue));
  }

  init() {
    this.filterGroup = '';
    this.searchString = '';
    this.sort = 'Group';
    this.pageSize = 10;
    this.pageNumber = 1;
  }

  submitChanges(): void {
    this.loading = true;

      this.productsService.getProducts(this.filterGroup, this.searchString, this.sort, this.pageSize, this.pageNumber)
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

    showModalProductsPrices(name:string, id:number) {
      this.modalProductsPrices = true;
      this.modalProductName = name;
      this.modalProductId = id;
  /*
      var myDialog: HTMLDialogElement = this.elRef.nativeElement.querySelector('#myDialog');
      myDialog.show()*/
    }

    closeModalProductsPrices() {
      this.modalProductsPrices = false;
  /*
      var myDialog: HTMLDialogElement = this.elRef.nativeElement.querySelector('#myDialog');
      myDialog.close()*/
    }

    onGroupChange() {
      if (this.filterGroup != '') {
          this.searchString = ''
      }

      this.productsService.getProductsNames(this.filterGroup).subscribe((data: any) => {
        this.productsNames = data;
        console.log(this.productsNames.length)
      });
    }

    changePageSize() {
      this.pageNumber = 1
      this.submitChanges()
    }
}
