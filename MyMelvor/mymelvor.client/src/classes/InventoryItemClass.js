export default class InventoryItemClass {
    Id = null;
    Name = "";
    Description = "";
    Count = 0;

    constructor(id, name, description, count) {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.Count = count;
    }
}