import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})

export class UserService {
    users: Array<any> = [
        {id:1, name:'John', email:'john@mail.com'},
        {id:2, name:'Sam', email:'sam@mail.com'},
        {id:3, name:'Smith', email:'smith@mail.com'},
        {id:4, name:'Raj', email:'raj@mail.com'},
      ]

      constructor() {
        console.log('User Service new instance created');
      }
}