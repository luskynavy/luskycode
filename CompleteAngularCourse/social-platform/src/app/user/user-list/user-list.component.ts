import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent implements OnInit{

  users: any[] = [];

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.refresUsers();
  }

  refresUsers(): void {
    this.userService.getUsers().subscribe(users => this.users = users);
  }

}
