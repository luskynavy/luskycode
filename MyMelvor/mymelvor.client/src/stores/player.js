import { reactive } from 'vue'
import InventoryItemClass from "../classes/InventoryItemClass.js"

export const player = reactive({
    xp: 0,
    money: 0,
    cookingLevel: 0,
    woodcuttingLevel: 0,
    inventory: [],
    loadValues() {
        this.xp = 170;
        this.money = 5000;
        this.cookingLevel = 5;
        this.woodcuttingLevel = 3;
        this.inventory = [
            new InventoryItemClass(1, 'Wood', 'A wood log', 1),
            new InventoryItemClass(2, 'Fish', 'A fish', 1),
            new InventoryItemClass(7, 'Teak', 'A teak wood log', 10),
            new InventoryItemClass(3, 'Catfish', 'A catfish', 1)];
    }
})