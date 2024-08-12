import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [],
  template: `
   <p>The user's name is {{occupation}}</p>
  `,
  styles: ``
})
export class UserComponent {
  @Input() occupation = 'init';
}
