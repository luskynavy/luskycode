<script setup lang="ts">
    import { ref } from 'vue'
    import { player } from '../stores/player'
    import { typeColor, ItemTypeColor } from '@/classes/ItemTypeColor'

    const hoverId = ref(-1)
    const filterId = ref(-1)

    //Return dicovered array based on current filter
    function filterDiscovered() {
        if (filterId.value == -1) {
            return player.discovered
        }
        return player.discovered.filter((x) => x.Type==filterId.value)
    }
</script>

<template>
    <main>
        <span>Stats.vue</span>
        <br>
        <div class="items">
            <span :class="filterId==-1 ? 'selectedItem' : ''" class="filterButton p-1" @click="filterId=-1"> All</span>
            <span v-for="filterButton in ItemTypeColor" :key="filterButton.Type" 
            :class="filterId==filterButton.Type ? 'selectedItem' : ''" 
            :style="{background: typeColor(filterButton.Type)}"
            class="filterButton p-1"
             @click="filterId=filterButton.Type">{{ filterButton.Name }}</span>
        </div>
        <br>
        <div class="items">
            <span v-for="element in filterDiscovered()" :key="element.Id" class="item p-1"
                :style="{background: typeColor(element.Type)}"
                @mouseover="hoverId = element.Id" @mouseleave="hoverId = -1">
                <div  class="d-inline-flex flex-column">
                    <span :class="element.Count>0 ? 'selectedItem' : ''">{{element.Name}} x {{element.Count}}</span>
                    <span v-if="hoverId==element.Id" class="m-2">
                        {{element.Description}}
                    </span>
                </div>
            </span>
        </div>
    </main>
</template>

<style scoped>
    .selectedItem {
        font-weight: bold;
    }

    .items {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }

    .item {
        width: 150px;
        height: 150px;
        border: 1px solid black;
    }
    
    .filterButton {
        width: 150px;    
        border: 1px solid black;
    }
</style>