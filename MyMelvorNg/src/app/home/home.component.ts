import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  player = inject(PlayerService)
}
