import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class PlayerService {
    xp = 0;
    money = 0;
    cookingLevel = 0;
    fishingLevel = 0;
    woodcuttingLevel = 0;

    loadValues() {
        this.xp = 170
        this.money = 5000
        this.cookingLevel = 5
        this.fishingLevel = 4
        this.woodcuttingLevel = 3
        /*this.inventory = [
            new InventoryItemClass(ItemId.Wood, 1),
            new InventoryItemClass(ItemId.RawFish, 1),
            new InventoryItemClass(ItemId.Teak, 10),
            new InventoryItemClass(ItemId.Catfish, 2)]*/
    }
}
