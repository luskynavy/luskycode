import { Component } from '@angular/core';

@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [],
  template: `
    <span> Comments:</span>
    <ul>
     <li>Building for the web is fantastic!!!</li>
     <li>The new template syntax is great</li>
     <li>I agree with the other comments!</li>
    </ul>
  `,
  styles: ``
})
export class CommentsComponent {

}
