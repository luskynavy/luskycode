<script setup>
    //import { player } from '../stores/player'
    import ItemId from "../classes/ItemId"
    import { usePlayerStore } from '../stores/player'

    const player = usePlayerStore()

    function getFish(idItem) {
        player.addToInventory(idItem, 1)
        player.fishingLevel++
    }
</script>

<template>
    <main>
        <div>Fishing.vue</div>
        <div>Fishing level :  {{player.fishingLevel}}</div>
        <span class="p-2">
            <span class="p-1">You have {{ player.getNbItemInInventory(ItemId.RawFish) }} raw fish</span>        
            <button @click="getFish(ItemId.RawFish)">Get 1 raw fish</button>
        </span>
        <span class="p-2">
            <span class="p-1" v-if="player.fishingLevel >= 5">You have {{ player.getNbItemInInventory(ItemId.RawCatfish) }} raw catfish</span>
            <button v-if="player.fishingLevel >= 5" @click="getFish(ItemId.RawCatfish)">Get 1 raw catfish</button>
        </span>
    </main>
</template>