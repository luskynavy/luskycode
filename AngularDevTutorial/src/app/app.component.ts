import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { TestsComponent } from "./tests/tests.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, TestsComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {

}
