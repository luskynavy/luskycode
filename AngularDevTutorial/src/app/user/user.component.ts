import { Component, Input, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { CarService } from '../car.service';
import { LowerCasePipe, DecimalPipe, DatePipe, CurrencyPipe } from '@angular/common';
import { ReversePipe } from '../reverse.pipe';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, LowerCasePipe, DecimalPipe, DatePipe, CurrencyPipe, ReversePipe],
  template: `
    <p>The occupation user's name is {{occupation}}</p>
    <p>{{ username | lowercase }}'s favorite framework: {{ favoriteFramework }}</p>
    <label for="framework">
      Favorite framework:
      <input id="framework" type="text" [(ngModel)]="favoriteFramework"/>
    </label>
    <button (click)="showFramework()">Show framework</button>
    <br>
    <br>
    <form [formGroup]="profileForm" (ngSubmit)="handleSubmit()">
      <label>
        Name
        <input type="text" formControlName="name" />
      </label>
      <br>
      <label>
        Email
        <input type="email" formControlName="email" />
      </label>
      <button type="submit" [disabled]="!profileForm.valid">Submit</button>
    </form>

    <br>
    <h2>Profile Form</h2>
    <p>Name: {{ profileForm.value.name }}</p>
    <p>Email: {{ profileForm.value.email }}</p>

    <br>
    <p>Car Listing: {{ display }}</p>

    <br>
    <ul>
      <li>Number with "decimal" {{ num | number:"3.2-2"}}</li>
      <li>Date with "date" {{ birthday | date:'medium'}}</li>
      <li>Currency with "currency" {{ cost | currency}}</li>
      <li>Reverse of lowercase of {{username}} is {{username | lowercase | reverse}}</li>
    </ul>
  `,
  styles: ``
})
export class UserComponent {
  @Input() occupation = 'default';

  username = 'yOUngtEch';
  favoriteFramework = '';

  profileForm = new FormGroup({
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
  });

  //syntax with inject
  //carService = inject(CarService)

  display = '';

  num = 103.1234;
  birthday = new Date(2023, 3, 2);
  cost = 4560.34;

  //syntax without inject
  constructor(private carService: CarService) {
    this.display = this.carService.getCars().join(' ⭐️ ');
  }

  showFramework() {
    alert(this.favoriteFramework)
  }

  handleSubmit() {
    alert(this.profileForm.value.name + ' | ' + this.profileForm.value.email);
  }
}
