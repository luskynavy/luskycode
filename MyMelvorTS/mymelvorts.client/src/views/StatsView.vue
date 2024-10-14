<script setup lang="ts">
    import { ref } from 'vue'
    import { player } from '../stores/player'
    import { typeColor} from '@/classes/ItemTypeColor'

    const hoverId = ref(-1)
</script>

<template>
    <main>
        <span>Stats.vue</span>
        <div class="items">
            <span v-for="element in player.discovered" :key="element.Id" class="item p-1"
                :style="{background: typeColor(element.Type)}"
                @mouseover="hoverId = element.Id" @mouseleave="hoverId = -1">
                <div class="d-inline-flex flex-column">
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
</style>