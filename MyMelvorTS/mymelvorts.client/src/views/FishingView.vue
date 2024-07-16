<script setup lang="ts">
    import { player } from '../stores/player'
    import ItemId from "../classes/ItemId"

    function getFish(idItem:number) {
        player.addToInventory(idItem, 1)
        player.fishingLevel++
    }
</script>

<template>
    <main>
        <div>Fishing.vue</div>
        <div>Fishing level :  {{player.fishingLevel}}</div>
        <button @click="getFish(ItemId.RawFish)">Get 1 raw fish</button>
        <button v-if="player.fishingLevel >= 5" @click="getFish(ItemId.RawCatfish)">Get 1 raw catfish</button>
    </main>
</template>