<script setup>
    import { player } from '../stores/player.js'
</script>

<template>
    <main>
        <div>Cooking.vue</div>
        <div>Cooking level :  {{player.cookingLevel}}</div>
        <button @click="player.cookingLevel++">Cook 1 fish</button>
        <button v-if="player.cookingLevel >= 5" @click="player.cookingLevel++">Cook 1 catfish</button>
    </main>
</template>