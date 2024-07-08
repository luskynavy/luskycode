<script setup>
    import { player } from '../stores/player.js'
</script>

<template>
    <main>
        <div>Woodcutting.vue</div>
        <div>Woodcutting level :  {{player.woodcuttingLevel}}</div>
        <button @click="player.woodcuttingLevel++">Cut 1 normal log</button>
        <button v-if="player.woodcuttingLevel >= 5" @click="player.woodcuttingLevel++">Cut 1 teak log</button>
    </main>
</template>