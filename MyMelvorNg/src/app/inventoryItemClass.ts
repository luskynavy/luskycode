import ItemArray from "./itemArray"

export default class InventoryItemClass {
    Id: number = -1
    Name: string = ""
    Description: string = ""
    Count: number = 0

    constructor(id: number, count: number) {
        this.Id = id
        this.Count = count

        const foundItem = ItemArray.find(i => i.Id == id)
        if (foundItem !== undefined) {
            this.Name = foundItem.Name
            this.Description = foundItem.Description
        }
    }
}