import ItemType from "./ItemType"

class TypeColor {
    Type = null
    Color = ""
    Name =""

    constructor(type, color, name) {
        this.Type = type
        this.Color = color
        this.Name = name
    }
}

export const ItemTypeColor =
    [
        new TypeColor(ItemType.Wood, "#deb887", "Wood"),
        new TypeColor(ItemType.RawFood, "#c0c0c0", "Raw Food"),
        new TypeColor(ItemType.CookedFood, "#e9967a", "Cooked Food"),
    ]

export function typeColor(type) {
    const color = ItemTypeColor.find(i => i.Type == type)
    return color?.Color
}