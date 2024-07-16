<script setup lang="ts">
    import { player } from '../stores/player'
    import ItemId from "../classes/ItemId"

    //Cook a raw fish to a fish
    function cookFish(idRawItem:number, idItem:number) {
        player.addToInventory(idRawItem, -1)
        player.addToInventory(idItem, 1)
        player.cookingLevel++
    }    
</script>

<template>
    <main>
        <div>Cooking.vue</div>
        <div>Cooking level :  {{player.cookingLevel}}</div>
        <button v-if="player.hasItemInInventory(ItemId.RawFish)" @click="cookFish(ItemId.RawFish, ItemId.Fish)">Cook 1 fish</button>
        <button v-if="player.cookingLevel >= 5 && player.hasItemInInventory(ItemId.RawCatfish)" @click="cookFish(ItemId.RawCatfish, ItemId.Catfish)">Cook 1 catfish</button>
    </main>
</template>