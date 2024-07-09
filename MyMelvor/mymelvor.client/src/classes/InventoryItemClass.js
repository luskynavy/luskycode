import ItemArray from "../classes/ItemArray.js"

export default class InventoryItemClass {
    Id = null
    Name = ""
    Description = ""
    Count = 0

    constructor(id, count) {
        this.Id = id
        this.Count = count

        var foundItem = ItemArray.find(i => i.Id == id)
        if (foundItem !== undefined) {
            this.Name = foundItem.Name
            this.Description = foundItem.Description
        }
    }
}