import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PlayerService } from '../player.service';
import InventoryItemClass from '../inventoryItemClass';
import { SortablejsModule } from 'nxt-sortablejs'
import Swal from 'sweetalert2';
import { typeColor} from '../itemTypeColor'

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [FormsModule, SortablejsModule],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.css'
})
export class InventoryComponent {
  player = inject(PlayerService);

  hoverId = -1
  selectedId = -1
  range = 1
  selectedItem: InventoryItemClass | undefined

  //Change selected infos
  selectItem(itemId: number) {
    this.selectedId = itemId
    this.selectedItem = this.player.inventory.find(i => i.Id == itemId)
    this.range = 1
  }

  typeColor(itemId: number) {
    return typeColor(itemId)
  }

  //Sell selected nb 'range' item
  sellItem() {
    const message = this.range + " " + this.selectedItem?.Name

    Swal.fire({
        text: "Sell " + message + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
    }).then((result) => {
        if (result.isConfirmed) {
            this.player.sellItem(this.selectedId, this.range)

            let itemRemaining = this.player.inventory.find(i => i.Id == this.selectedId)

            //Remove selection if all items sold
            if (itemRemaining == undefined) {
              this.selectedId = -1
              this.selectedItem = undefined
              this.range = 1
            }
            //Range value can't be more than item count
            else if (this.range > itemRemaining.Count) {
              this.range = itemRemaining.Count
            }

            Swal.fire({
                text: "You have sold " + message,
                icon: "success"
            })
        }
    })
  }
}
