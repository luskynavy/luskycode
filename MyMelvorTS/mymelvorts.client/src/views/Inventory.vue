<script setup lang="ts">
    import { ref } from 'vue'
    import { player } from '../stores/player'
    const hoverId = ref(-1);
</script>

<template>
    <main>
        <span>Inventory.vue</span>
        <div v-for="item in player.inventory" :key="item.Id">
            <span @mouseover="hoverId = item.Id" @mouseleave="hoverId = -1">{{item.Name}} x {{item.Count}}</span>
            <span v-if="hoverId==item.Id">
                {{item.Description}}
            </span>
        </div>
    </main>    
</template>