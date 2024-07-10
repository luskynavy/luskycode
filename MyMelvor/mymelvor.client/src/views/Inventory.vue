<script setup>
    import { ref } from 'vue'
    import { player } from '../stores/player.js'    
    const hoverId = ref(null);
</script>

<template>
    <main>
        <span>Inventory.vue</span>
        <div v-for="item in player.inventory" :key="item.Id">
            <span @mouseover="hoverId = item.Id" @mouseleave="hoverId = null">{{item.Name}} x {{item.Count}}</span>
            <span v-if="hoverId==item.Id">
                {{item.Description}}
            </span>
        </div>
    </main>
</template>