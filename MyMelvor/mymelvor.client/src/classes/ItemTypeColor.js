import ItemType from "./ItemType"

class TypeColor {
    Type = null
    Color = ""

    constructor(type, color) {
        this.Type = type
        this.Color = color
    }
}

export const ItemTypeColor =
    [
        new TypeColor(ItemType.Wood, "#deb887" ),
        new TypeColor(ItemType.RawFood, "#c0c0c0"),
        new TypeColor(ItemType.CookedFood, "#e9967a"),
    ]

export function typeColor(type) {
    const color = ItemTypeColor.find(i => i.Type == type)
    return color?.Color
}