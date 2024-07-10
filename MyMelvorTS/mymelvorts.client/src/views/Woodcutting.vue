<script setup lang="ts">
    import { player } from '../stores/player.ts'
    import ItemId from "../classes/ItemId.ts"

    function cutWood(idItem:number) {
        player.addToInventory(idItem, 1)
        player.woodcuttingLevel++
    }
</script>

<template>
    <main>
        <div>Woodcutting.vue</div>
        <div>Woodcutting level :  {{player.woodcuttingLevel}}</div>
        <button @click="cutWood(ItemId.Wood)">Cut 1 normal log</button>
        <button v-if="player.woodcuttingLevel >= 5" @click="cutWood(ItemId.Teak)">Cut 1 teak log</button>
    </main>
</template>