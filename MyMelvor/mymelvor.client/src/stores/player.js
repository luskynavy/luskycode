import { reactive } from 'vue'
import InventoryItemClass from "../classes/InventoryItemClass.js"
import ItemId from "../classes/ItemId.js"
import ItemArray from "../classes/ItemArray.js"

export const player = reactive({
    xp: 0,
    money: 0,
    cookingLevel: 0,
    woodcuttingLevel: 0,
    inventory: [],

    //Load default values for test
    loadValues() {
        this.xp = 170;
        this.money = 5000;
        this.cookingLevel = 5;
        this.woodcuttingLevel = 3;
        this.inventory = [
            new InventoryItemClass(ItemId.Wood, 1),
            new InventoryItemClass(ItemId.Fish, 1),
            new InventoryItemClass(ItemId.Teak, 10),
            new InventoryItemClass(ItemId.Catfish, 2)]
    },

    //Add nb count item of id idItem
    addToInventory(idItem, count) {
        //Search the item in inventory
        var foundInventory = this.inventory.find(i => i.Id == idItem)

        //Create the item if not found
        if (foundInventory === undefined) {
            var foundItem = ItemArray.find(i => i.Id == idItem)
            if (foundItem !== undefined) {
                this.inventory.push(new InventoryItemClass(idItem, count))
            }
        }
        //Update item count if found
        else {
            foundInventory.Count += count
        }
    }
})