import { Component, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-posts-list',
  standalone: true,
  imports: [],
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

  sendMessage() {
    console.log('Button clicked')
    this.MessageEvent.emit(this.parentMessage);
  }
}
