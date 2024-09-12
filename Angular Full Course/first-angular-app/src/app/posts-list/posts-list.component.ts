import { Component, Input, EventEmitter, Output } from '@angular/core';
import { CardComponent } from "../card/card.component";
import { UserService } from '../services/user.service';
import { JsonPipe } from '@angular/common';
import { PostService /*, Post*/ } from '../services/post.service';
import {Post} from '../interfaces/post'


@Component({
  selector: 'app-posts-list',
  standalone: true,
  imports: [JsonPipe, CardComponent],
  templateUrl: './posts-list.component.html',
  styleUrl: './posts-list.component.css'
})
export class PostsListComponent {
  @Input() postListTitle: string = '';
  @Input() postListIsLogin: boolean = false;

  childMessage: string = 'Hello from child component';
  postCount: number= 0;
  parentMessage: string = 'Message from the child using click event';

  @Output() MessageEvent = new EventEmitter();

  userService : any
  posts:Array<any> = []

  constructor(private userServiceDI:UserService, private postService:PostService) {
    //manually create user service
    //this.userService = new UserService()

    //create user service from dependency injection
    this.userService = userServiceDI

    this.posts = postService.getPost()
  }

  sendMessage() {
    console.log('Button clicked')
    this.MessageEvent.emit(this.parentMessage);
  }

  addPost() {
    let postData:Post = {
      id: 7,
      title: 'Post Title 7',
      post:'Dummy Post 7'
    }

    this.postService.addPostService(postData)
  }

}
