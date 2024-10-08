import { reactive } from 'vue'
import InventoryItemClass from "../classes/InventoryItemClass"
import ItemId from "../classes/ItemId"
import ItemArray from "../classes/ItemArray"

export const player = reactive({
    xp: 0,
    money: 0,
    cookingLevel: 0,
    fishingLevel: 0,
    woodcuttingLevel: 0,
    inventory: [] as InventoryItemClass[],
    discovered: [
        new InventoryItemClass(ItemId.Wood, 0),
        new InventoryItemClass(ItemId.Teak, 0),
        new InventoryItemClass(ItemId.Fish, 0),
        new InventoryItemClass(ItemId.Catfish, 0),
        new InventoryItemClass(ItemId.RawFish, 0),
        new InventoryItemClass(ItemId.RawCatfish, 0)
    ],

    //Load default values for test
    loadValues() {
        this.xp = 170
        this.money = 5000
        this.cookingLevel = 5
        this.fishingLevel = 4
        this.woodcuttingLevel = 3

        this.discovered = [
            new InventoryItemClass(ItemId.Wood, 0),
            new InventoryItemClass(ItemId.Teak, 0),
            new InventoryItemClass(ItemId.Fish, 0),
            new InventoryItemClass(ItemId.Catfish, 0),
            new InventoryItemClass(ItemId.RawFish, 0),
            new InventoryItemClass(ItemId.RawCatfish, 2)
        ],

        this.addToInventory(ItemId.Wood, 1),
        this.addToInventory(ItemId.RawFish, 1),
        this.addToInventory(ItemId.Teak, 10),
        this.addToInventory(ItemId.Catfish, 2)
    },

    //Add nb 'count' item of id 'idItem'
    addToInventory(idItem: number, count: number) {
        //Search the item in inventory
        const foundInventory = this.inventory.find(i => i.Id == idItem)

        //Create the item if not found
        if (foundInventory === undefined) {
            const foundItem = ItemArray.find(i => i.Id == idItem)
            if (foundItem !== undefined) {
                this.inventory.push(new InventoryItemClass(idItem, count))
            }
        }
        //Update item count if found
        else {
            foundInventory.Count += count

            //Remove item if quantity <= 0
            if (foundInventory.Count <= 0) {
                const index = this.inventory.indexOf(foundInventory)
                this.inventory.splice(index, 1)
            }
        }

        //Don't remove item
        if (count >= 0)
        {
            //Search the item in discovered
            const foundDiscovered = this.discovered.find(i => i.Id == idItem)

            //Create the item if not found
            if (foundDiscovered === undefined) {
                const foundItem = ItemArray.find(i => i.Id == idItem)
                if (foundItem !== undefined) {
                    this.discovered.push(new InventoryItemClass(idItem, count))
                }
            }
            //Update item count if found
            else {
                foundDiscovered.Count += count

                //Remove item if quantity <= 0
                if (foundDiscovered.Count <= 0) {
                    const index = this.discovered.indexOf(foundDiscovered)
                    this.discovered.splice(index, 1)
                }
            }
        }
    },

    //True if has item in inventory
    hasItemInInventory(idItem: number) {
        return this.inventory.find(i => i.Id == idItem) !== undefined
    },

    //Get the total number of items in inventory
    getTotalItemsInInventory() {
        let nb = 0
        for (let index = 0; index < this.inventory.length; index++) {
             nb += this.inventory[index].Count;
        }
        return nb
    },

    //Get item count in inventory
    getNbItemInInventory(idItem: number) {
        const foundInventory = this.inventory.find(i => i.Id == idItem)
        if (foundInventory === undefined) {
            return 0
        }
        else {
            return foundInventory.Count
        }
    },

    //Sell nb 'count' item of id 'idItem'
    sellItem(idItem: number, count: number) {
        this.addToInventory(idItem, -count)
        this.money += count * 100
    }
})