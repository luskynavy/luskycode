<script setup>
    import { player } from '../stores/player.js'
</script>

<template>
    <main>
        <div>Cooking.vue</div>
        <div>Cooking level :  {{player.cookingLevel}}</div>
        <button @click="player.cookingLevel++">Add 1</button>
    </main>
</template>