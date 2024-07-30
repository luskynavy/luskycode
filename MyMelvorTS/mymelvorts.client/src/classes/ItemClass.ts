export default class ItemClass {
    Id: number = -1
    Name: string = "";
    Description: string = ""

    constructor(id: number, name: string, description: string) {
        this.Id = id
        this.Name = name
        this.Description = description
    }
}