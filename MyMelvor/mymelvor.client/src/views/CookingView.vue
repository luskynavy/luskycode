<script setup>
    //import { player } from '../stores/player.js'
    import { usePlayerStore } from '../stores/player'
    import ItemId from "../classes/ItemId.js"

    const player = usePlayerStore()

    function cookFish(idItem) {
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