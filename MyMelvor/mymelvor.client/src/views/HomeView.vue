<script setup>
    /*import TheWelcome from '../components/TheWelcome.vue'*/
    //import { player } from '../stores/player'
    import { usePlayerStore } from '../stores/player'

    const player = usePlayerStore()
</script>

<template>
    <main>

        <div>HomeView.vue</div>
        <div>Xp : {{player.xp}}</div>
        <div>Money : {{player.money}}</div>
        <div>Fishing level : {{player.fishingLevel}}</div>
        <div>Cooking level : {{player.cookingLevel}}</div>
        <div>Woodcutting level : {{player.woodcuttingLevel}}</div>

        <button @click="player.loadValues()">Load values</button>
    </main>
</template>