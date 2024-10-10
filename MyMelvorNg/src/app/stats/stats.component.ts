import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';
import { typeColor} from '../itemTypeColor'

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

  typeColor(itemId: number) {
      return typeColor(itemId)
  }
}
