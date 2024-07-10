<script setup lang="ts">
    import { player } from '../stores/player.ts'
    import ItemId from "../classes/ItemId.ts"

    function cookFish(idItem:number) {
        player.addToInventory(idItem, 1)
        player.cookingLevel++
    }
</script>

<template>
    <main>
        <div>Cooking.vue</div>
        <div>Cooking level :  {{player.cookingLevel}}</div>
        <button @click="cookFish(ItemId.Fish)">Cook 1 fish</button>
        <button v-if="player.cookingLevel >= 5" @click="cookFish(ItemId.Catfish)">Cook 1 catfish</button>
    </main>
</template>