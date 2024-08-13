import { Component, inject } from '@angular/core';
import { PlayerService } from '../player.service';
import { ItemId } from '../itemid.enum';

@Component({
  selector: 'app-cooking',
  standalone: true,
  imports: [],
  templateUrl: './cooking.component.html',
  styleUrl: './cooking.component.css'
})
export class CookingComponent {
  player = inject(PlayerService);
  itemId = ItemId;

   //Cook a raw fish to a fish
   cookFish(idRawItem: number, idItem: number) {
    this.player.addToInventory(idRawItem, -1)
    this.player.addToInventory(idItem, 1)
    this.player.cookingLevel++
  }
}
