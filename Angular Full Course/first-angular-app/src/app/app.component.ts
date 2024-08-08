import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppNavbar } from "./navbar/navbar.component";
import { HeaderComponent } from "./header/header.component";
import { FormsModule } from '@angular/forms';
import { NgIf, NgTemplateOutlet, NgFor, NgSwitch, NgSwitchCase, NgSwitchDefault, NgStyle, NgClass} from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AppNavbar, HeaderComponent, FormsModule, NgIf, NgTemplateOutlet, NgFor, NgSwitch, NgSwitchCase, NgSwitchDefault, NgStyle, NgClass],
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


  users: Array<string> = ["John", "Sam", "Smith", "Raj"];

  usersObj: Array<any> = [
    {id:1, name:'John', email:'john@mail.com'},
    {id:2, name:'Sam', email:'sam@mail.com'},
    {id:3, name:'Smith', email:'smith@mail.com'},
    {id:4, name:'Raj', email:'raj@mail.com'},
  ]

  constructor() {
    console.log(this.usersObj.length);
  }

  addNewUser() {
    let id = this.usersObj.length + 1;
    let user =  {id:id, name:'User '+ id, email:'user'+ id + '@mail.com'};
    this.usersObj.push(user);
  }

  onDelete(user:any) {
    let index = this.usersObj.indexOf(user);
    console.log(index);
    this.usersObj.splice(index, 1);
  }

  onDeleteIndex(i:number) {
    console.log(i);
    this.usersObj.splice(i, 1);
  }

  clearUserRole() {
    this.userRole = '';
  }
}

