import { Component } from '@angular/core';
import { UserComponent } from '../user/user.component';
import { ChildComponent } from '../child/child.component';
import { CommentsComponent } from "../comments/comments.component";
import { NgOptimizedImage } from '@angular/common';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-tests',
  standalone: true,
  imports: [UserComponent, ChildComponent, CommentsComponent, NgOptimizedImage, RouterOutlet],
  templateUrl: './tests.components.html',
  styles: ``
})
export class TestsComponent {
  title = 'default';
  city = 'San Francisco';
  isLoggedIn = true;
  isServerRunning = false;
  operatingSystems = [{id: 'win', name: 'Windows'}, {id: 'osx', name: 'MacOS'}, {id: 'linux', name: 'Linux'}];
  users = [{id: 0, name: 'Sarah'}, {id: 1, name: 'Amy'}, {id: 2, name: 'Rachel'}, {id: 3, name: 'Jessica'}, {id: 4, name: 'Poornima'}];
  isEditable = true;
  message = '';
  logoUrl = '/assets/logo.svg';
  logoAlt = 'Angular logo';

  onMouseOver() {
    this.message = 'Way to go 🚀';
  }
  greet() {
    console.log('Hello, there 👋')
  }

  items = new Array();

  addItem(item: string) {
    this.items.push(item);
  }
}
