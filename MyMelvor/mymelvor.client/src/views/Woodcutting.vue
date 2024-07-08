<script setup>
    import { player } from '../stores/player.js'
</script>

<template>
    <main>
        <div>Woodcutting.vue</div>
        <div>Woodcutting level :  {{player.woodcuttingLevel}}</div>
        <button @click="player.woodcuttingLevel++">Add 1</button>
    </main>
</template>