import ItemArray from "../classes/ItemArray"
import ItemClass from "./ItemClass"
import ItemType from "./ItemType"

export default class InventoryItemClass extends ItemClass {
    Count: number = 0

    constructor(id: number, count: number) {
        super(id, "", "", ItemType.Wood)
        this.Count = count

        const foundItem = ItemArray.find(i => i.Id == id)
        if (foundItem !== undefined) {
            this.Name = foundItem.Name
            this.Description = foundItem.Description
            this.Type = foundItem.Type
        }
    }
}