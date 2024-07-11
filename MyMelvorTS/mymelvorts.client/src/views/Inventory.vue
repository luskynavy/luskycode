<script setup lang="ts">
    import { ref } from 'vue'
    import { player } from '../stores/player'
    const hoverId = ref(-1)
    const selectedId = ref(-1)
</script>

<template>
    <main>
        <span>Inventory.vue</span>
        <div class="d-inline-flexZZ items">
            <div v-for="item in player.inventory" :key="item.Id" class="item p-1">
                <div class="d-inline-flex flex-column">
                    <span @mouseover="hoverId = item.Id" @mouseleave="hoverId = -1"
                    @click="selectedId=item.Id" :class="selectedId==item.Id ? 'selectedItem' : ''">{{item.Name}} x {{item.Count}}</span>
                    <span v-if="hoverId==item.Id" class="m-2">
                        {{item.Description}}
                    </span>
                </div>
            </div>
        </div>
    </main>    
</template>

<style scoped>
.selectedItem {
    font-weight:bold;
}

.items{
    display:flex;
    flex-direction:row;
    flex-wrap:wrap;
}


.item{    
    width:150px;
    height:150px;
}
</style>