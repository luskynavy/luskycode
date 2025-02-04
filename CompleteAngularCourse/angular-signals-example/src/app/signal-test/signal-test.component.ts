import { Component } from '@angular/core';
import { signal, effect, computed } from '@angular/core';

@Component({
  selector: 'app-signal-test',
  standalone: true,
  imports: [],
  templateUrl: './signal-test.component.html',
  styleUrl: './signal-test.component.css'
})
export class SignalTestComponent {
  title = 'angular-signals-example';

  theme = signal('light');
  label = this.theme();

  price = 19;
  quantity = signal(10);
  totalPrice = computed(() => this.price * this.quantity());

  constructor() {
    effect( () => {
      this.label = this.theme();
    });
  }

  toggleDarkMode() {
    this.theme.update(currentValue => currentValue === 'light' ? 'dark' : 'light');
  }

  changeQuantity(event: Event) {
    this.quantity.set((event.target as HTMLInputElement).valueAsNumber)
  }
}
