<script setup lang="ts">
    import { player } from '../stores/player'
</script>

<template>
    <main>
        <div class="d-flex flex-column align-items-start">
            <div>HomeView.vue</div>
            <div>Xp : {{player.xp}}</div>
            <div>Money : {{player.money}}</div>
			<div>Woodcutting level :  {{player.woodcuttingLevel}}</div>
            <div>Fishing level : {{player.fishingLevel}}</div>
            <div>Cooking level : {{player.cookingLevel}}</div>            
        </div>
        <button @click="player.loadValues()">Load values</button>
    </main>
</template>