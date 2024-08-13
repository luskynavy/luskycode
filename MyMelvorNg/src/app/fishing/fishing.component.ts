import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';

@Component({
  selector: 'app-fishing',
  standalone: true,
  imports: [],
  templateUrl: './fishing.component.html',
  styleUrl: './fishing.component.css'
})
export class FishingComponent {
  player = inject(PlayerService)

  getFish(/*idItem: number*/) {
    //this.player.addToInventory(idItem, 1)
    this.player.fishingLevel++;
  }
}
