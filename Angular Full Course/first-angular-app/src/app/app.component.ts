import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppNavbar } from "./navbar/navbar.component";
import { HeaderComponent } from "./header/header.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AppNavbar, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title: string = 'This loaded dynamically';
  imgURL: string = 'https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/Angular_full_color_logo.svg/100px-Angular_full_color_logo.svg.png'
  isDisabled: boolean = true;
  isActive: boolean = true;
  fruitName: string = 'Apple';
  onButton() {
    console.log('mouseover');
  }

  buttonClick() {
    console.log("button click")
  }

  keyEnter(event:any) {
    //console.log(event.keyCode);

    if (event.keyCode == 13) {
      console.log('Enter key pressed');
    }
  }

  keyupFiltering() {
    console.log('Enter key pressed');
  }
}

