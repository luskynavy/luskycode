import ItemArray from "./itemArray"
import ItemClass from "./itemClass"
import ItemType from "./itemType"

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