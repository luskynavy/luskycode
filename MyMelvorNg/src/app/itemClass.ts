import type ItemType from "./itemType"

export default class ItemClass {
    Id: number = -1
    Name: string = "";
    Description: string = ""
    Type: ItemType

    constructor(id: number, name: string, description: string, type: ItemType) {
        this.Id = id
        this.Name = name
        this.Description = description
        this.Type = type
    }
}