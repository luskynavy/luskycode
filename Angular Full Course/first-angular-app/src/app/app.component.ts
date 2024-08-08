import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppNavbar } from "./navbar/navbar.component";
import { HeaderComponent } from "./header/header.component";
import { FormsModule } from '@angular/forms';
import { NgIf, NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AppNavbar, HeaderComponent, FormsModule, NgIf, NgTemplateOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title: string = 'This loaded dynamically';
  imgURL: string = 'https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/Angular_full_color_logo.svg/100px-Angular_full_color_logo.svg.png'
  isDisabled: boolean = true;
  isActive: boolean = true;
  fruitName: string = 'Apple';
  textValue: string = "some text";

  isLoggedIn:boolean = false;
  userName: string = "John Doe";

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

  keyupFiltering(user:HTMLInputElement) {
    console.log(user.value);
    //console.log('Enter key pressed');
  }

  updateUserName(username:HTMLInputElement) {
    this.userName = username.value
    console.log(this.userName);
  }

  onKeyup() {
    console.log(this.textValue);
  }

  toggleIsLogged() {
    this.isLoggedIn = !this.isLoggedIn;
  }

  loginCount: number= 0;
  userRole: string = "Admin";  
  countLoginAttempts() {
    this.loginCount++;
  }

  toggleRole() {
    if (this.userRole == 'Admin') {
      this.userRole = 'Member';
    } else {
      this.userRole = 'Admin';
    }
  }
}

