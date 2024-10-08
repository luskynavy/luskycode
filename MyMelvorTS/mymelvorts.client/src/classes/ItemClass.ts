import type ItemType from "./ItemType"

export default class ItemClass {
    Id: number = -1
    Name: string = "";
    Description: string = ""
    Type: ItemType

    constructor(id: number, name: string, description: string, itemType: ItemType) {
        this.Id = id
        this.Name = name
        this.Description = description
        this.Type = itemType
    }
}