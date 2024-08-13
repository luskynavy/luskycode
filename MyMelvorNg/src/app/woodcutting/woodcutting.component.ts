import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';
import { ItemId } from '../itemid.enum';

@Component({
  selector: 'app-woodcutting',
  standalone: true,
  imports: [],
  templateUrl: './woodcutting.component.html',
  styleUrl: './woodcutting.component.css'
})
export class WoodcuttingComponent {
  player = inject(PlayerService)
  itemId = ItemId;

  cutWood(idItem: number) {
    this.player.addToInventory(idItem, 1)
    this.player.woodcuttingLevel++
}
}
