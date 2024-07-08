import { reactive } from 'vue'

export const player = reactive({
    xp: 0,
    money: 0,
    cookingLevel: 0,
    woodcuttingLevel: 0,
    loadValues() {
        this.xp = 170;
        this.money = 5000;
        this.cookingLevel = 5;
        this.woodcuttingLevel = 3;
    }
})