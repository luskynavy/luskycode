import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';
import { typeColor, ItemTypeColor } from '../itemTypeColor'
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
  filterId = -1

  myItemTypeColor = ItemTypeColor

  typeColor(type: ItemType) {
      return typeColor(type)
  }

  //Return dicovered array based on current filter
  filterDiscovered() {
    if (this.filterId == -1) {
        return this.player.discovered
    }
    return this.player.discovered.filter((x) => x.Type==this.filterId)
  }
}
