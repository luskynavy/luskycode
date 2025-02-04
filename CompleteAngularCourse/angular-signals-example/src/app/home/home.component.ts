import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../product';
import { ProductListComponent } from '../product-list/product-list.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ProductListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  allProducts: Product[] = [
    {id: 1, name: "Milk", price: 1.45},
    {id: 2, name: "Bread", price: 3.90},
    {id: 3, name: "Tomatoes", price: 2.20},
  ]
}

