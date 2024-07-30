<script setup lang="ts">
    import { player } from '../stores/player'
    import ItemId from "../classes/ItemId"

    //Cook a raw fish to a fish
    function cookFish(idRawItem: number, idItem: number) {
        player.addToInventory(idRawItem, -1)
        player.addToInventory(idItem, 1)
        player.cookingLevel++
    }
</script>

<template>
    <main>
        <div>Cooking.vue</div>
        <div>Cooking level : {{player.cookingLevel}}</div>
        <span class="p-2">
            <span class="p-1">You have {{ player.getNbItemInInventory(ItemId.RawFish) }} raw fish and {{ player.getNbItemInInventory(ItemId.Fish) }} fish</span>
            <button v-if="player.hasItemInInventory(ItemId.RawFish)" @click="cookFish(ItemId.RawFish, ItemId.Fish)">Cook 1 fish</button>
        </span>
        <span class="p-2">
            <span class="p-1" v-if="player.cookingLevel >= 5">You have {{ player.getNbItemInInventory(ItemId.RawCatfish) }} raw catfish and {{ player.getNbItemInInventory(ItemId.Catfish) }} catfish</span>
            <button v-if="player.cookingLevel >= 5 && player.hasItemInInventory(ItemId.RawCatfish)" @click="cookFish(ItemId.RawCatfish, ItemId.Catfish)">Cook 1 catfish</button>
        </span>
    </main>
</template>