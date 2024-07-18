import ItemId from "../classes/ItemId"
import ItemClass from "../classes/ItemClass"

let ItemArray =
    [
        new ItemClass(ItemId.Wood, "Wood", "A wood log"),
        new ItemClass(ItemId.Teak, "Teak", "A teak log"),
        new ItemClass(ItemId.Fish, "Fish", "A fish"),
        new ItemClass(ItemId.Catfish, "Catfish", "A catfish"),
        new ItemClass(ItemId.RawFish, "Raw fish", "A raw fish"),
        new ItemClass(ItemId.RawCatfish, "Raw catfish", "A raw catfish")
    ]

export default ItemArray