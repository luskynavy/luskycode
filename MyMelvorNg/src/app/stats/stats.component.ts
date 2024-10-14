import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';
import { typeColor} from '../itemTypeColor'
import ItemType from '../itemType';

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

  typeColor(type: ItemType) {
      return typeColor(type)
  }
}
