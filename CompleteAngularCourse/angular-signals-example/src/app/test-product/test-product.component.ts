import { Component, computed, signal } from '@angular/core';

@Component({
  selector: 'app-test-product',
  standalone: true,
  imports: [],
  templateUrl: './test-product.component.html',
  styleUrl: './test-product.component.css'
})
export class TestProductComponent {
  products = signal([
    {id: 1, name: "Milk", price: 1.45},
    {id: 2, name: "Bread", price: 3.90},
    {id: 3, name: "Tomatoes", price: 2.20},
  ])

  filterName = signal('');

  filterProducts = computed(() => {
    return this.products().filter(
      product => product.name.toLocaleLowerCase()
      .includes(this.filterName().toLocaleLowerCase())
    );
  })

  changeFilter(event: Event) {
    let newFilterName = (event.target as HTMLInputElement).value;
    this.filterName.set(newFilterName);
  }
}
