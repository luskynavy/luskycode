import { Component, ViewChild, AfterViewInit, ViewContainerRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppNavbar } from "./navbar/navbar.component";
import { PostsListComponent} from "./posts-list/posts-list.component"
import { HeaderComponent } from "./header/header.component";
import { FormsModule } from '@angular/forms';
import { NgIf, NgTemplateOutlet, NgFor, NgSwitch, NgSwitchCase, NgSwitchDefault, NgStyle, NgClass, NgComponentOutlet, UpperCasePipe, TitleCasePipe, DecimalPipe, PercentPipe, CurrencyPipe, DatePipe, JsonPipe, SlicePipe} from '@angular/common';
import { CardComponent } from "./card/card.component";
import { ProfileComponent } from "./profile/profile.component";
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [SlicePipe, JsonPipe, DatePipe,CurrencyPipe, PercentPipe, DecimalPipe, TitleCasePipe, UpperCasePipe, RouterOutlet, AppNavbar, PostsListComponent, HeaderComponent, FormsModule, NgIf, NgTemplateOutlet, NgFor, NgSwitch, NgSwitchCase, NgSwitchDefault, NgStyle, NgClass, CardComponent, NgComponentOutlet, ProfileComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements AfterViewInit {
  title: string = 'This loaded dynamically';
  imgURL: string = 'https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/Angular_full_color_logo.svg/100px-Angular_full_color_logo.svg.png'
  isDisabled: boolean = true;
  isActive: boolean = true;
  fruitName: string = 'Apple';
  textValue: string = "some text";
  showCourses1_13: boolean = false;
  showCourses14_22: boolean = false;

  isLoggedIn:boolean = false;
  userName: string = "John Doe";

  appPostTitle: string = 'Post 1';
  appIsLogin: boolean = false;

  @ViewChild(PostsListComponent) childMessage : any;
  message:string = '';
  messageFromChild:string = '';

  testPipes: string = 'AngUlAr App'
  today = new Date()

  user: any = {
    name: 'John Doe',
    age : 30,
    email:'johndoe@mail.com'
  }

  user2: any = {
    name: 'John One',
    age : 30,
    email:'johnone@mail.com'
  }

  formSubmitJs(event:any) {
    event.preventDefault();
    console.log("Form Submitted");
    console.log(event.target.name.value);
  }

  formSubmit(event:any) {
    console.log("Form Submitted");
    console.log(event);
    console.log(event.value);
  }

  getValue(fullName:any) {
    console.log(fullName);
  }

  uppercase() {
    this.testPipes = this.testPipes.toUpperCase()
  }

  convertJson() {
    this.user = JSON.stringify(this.user)
  }

  ngAfterViewInit(): void {
    console.log('ngAfterViewInit');
    console.log(this.childMessage);
    this.message = this.childMessage.childMessage;
  }

  receiveMessage(message:string) {
    console.log(message)
    this.messageFromChild = message;
  }

 loadComponent() {
  //return PostsListComponent;
  this.viewContainer.createComponent(PostsListComponent);
 }

 loadComponentPofile() {
  this.viewContainer.createComponent(ProfileComponent);
 }

 removeComponent() {
  this.viewContainer.remove();
 }

 changeUser() {
  this.userName = "John Smith";
 }


  toggleCourses1_13() {
    this.showCourses1_13 = !this.showCourses1_13;
  }

  toggleCourses14_22() {
    this.showCourses14_22 = !this.showCourses14_22;
  }

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

  userService: any

  usersObj: Array<any> = [
    {id:1, name:'John', email:'john@mail.com'},
    {id:2, name:'Sam', email:'sam@mail.com'},
    {id:3, name:'Smith', email:'smith@mail.com'},
    {id:4, name:'Raj', email:'raj@mail.com'},
  ]

  constructor(private viewContainer : ViewContainerRef, private userServiceDI:UserService) {
    console.log(this.usersObj.length);

    console.log('app constructor');
    console.log(this.childMessage);

    //manually create user service
    //this.userService = new UserService()
    //console.log(this.userService)

    //create user service from dependency injection
    this.userService = userServiceDI
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

