import ItemId from "../classes/ItemId"
import ItemClass from "../classes/ItemClass"
import ItemType from "./ItemType"

let ItemArray =
    [
        new ItemClass(ItemId.Wood, "Wood", "A wood log", ItemType.Wood),
        new ItemClass(ItemId.Teak, "Teak", "A teak log", ItemType.Wood),
        new ItemClass(ItemId.Fish, "Fish", "A fish", ItemType.CookedFood),
        new ItemClass(ItemId.Catfish, "Catfish", "A catfish", ItemType.CookedFood),
        new ItemClass(ItemId.RawFish, "Raw fish", "A raw fish", ItemType.RawFood),
        new ItemClass(ItemId.RawCatfish, "Raw catfish", "A raw catfish", ItemType.RawFood)
    ]

export default ItemArray