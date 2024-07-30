<script setup lang="ts">
    import { player } from '../stores/player'
    import ItemId from "../classes/ItemId"

    function cutWood(idItem: number) {
        player.addToInventory(idItem, 1)
        player.woodcuttingLevel++
    }
</script>

<template>
    <main>
        <div>Woodcutting.vue</div>
        <div>Woodcutting level :  {{player.woodcuttingLevel}}</div>
        <span class="p-2">
            <span class="p-1">You have {{ player.getNbItemInInventory(ItemId.Wood) }} normal wood</span>
            <button @click="cutWood(ItemId.Wood)">Cut 1 normal log</button>
        </span>
        <span class="p-2">
            <span class="p-1" v-if="player.woodcuttingLevel >= 5">You have {{ player.getNbItemInInventory(ItemId.Teak) }} teak log</span>
            <button v-if="player.woodcuttingLevel >= 5" @click="cutWood(ItemId.Teak)">Cut 1 teak log</button>
        </span>
    </main>
</template>