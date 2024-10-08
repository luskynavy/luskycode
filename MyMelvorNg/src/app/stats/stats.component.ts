import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';

@Component({
  selector: 'app-stats',
  standalone: true,
  imports: [],
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.css'
})
export class StatsComponent {
  player = inject(PlayerService);

  hoverId = -1
}
