import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from '../services/products.service';
import { ProductPriceComponent } from '../product-price/product-price.component';

@Component({
  selector: 'app-details',
  standalone: true,
  imports: [ProductPriceComponent],
  templateUrl: './details.component.html',
  styleUrl: './details.component.css'
})
export class DetailsComponent {
  loading: boolean = false;
  product:any = {};

  id:number = 0;

   constructor(private route: ActivatedRoute, private productsService: ProductsService) {}

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    this.loading = true;

    this.productsService.getProduct(this.id).subscribe((data: any) => {
      this.product = data;
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

}
