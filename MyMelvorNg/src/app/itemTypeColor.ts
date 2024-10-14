
import ItemType from "./itemType"

class TypeColor {
    Type: ItemType
    Color: string =""
    constructor(itemType: ItemType, color: string) {
        this.Type = itemType
        this.Color = color
    }
}

export const ItemTypeColor =
    [
        new TypeColor(ItemType.Wood, "#deb887" ),
        new TypeColor(ItemType.RawFood, "#c0c0c0"),
        new TypeColor(ItemType.CookedFood, "#e9967a"),
    ]

export function typeColor(type: ItemType) {
    const color = ItemTypeColor.find(i => i.Type == type)
    return color?.Color
}