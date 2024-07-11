<script setup lang="ts">
    import { ref } from 'vue'
    import { player } from '../stores/player'
    import InventoryItemClass from "../classes/InventoryItemClass"
    const hoverId = ref(-1)
    const selectedId = ref(-1)
    const range = ref(1)
    var selectedItem :InventoryItemClass | undefined

    function selectItem(itemId:number) {
        selectedId.value=itemId
        selectedItem = player.inventory.find(i => i.Id == itemId)
        range.value = 1
    }

    function sellItem() {
        player.sellItem(selectedId.value, range.value)
        if (player.inventory.find(i => i.Id == selectedId.value) == undefined) {
            selectedId.value = -1
            selectedItem = undefined
            range.value = 1
        }
    }
</script>

<template>
    <main>
        <span>Inventory.vue</span>
        <section>
            <aside class="right">
                <div>
                    <span v-if="selectedId==-1">No item selected</span>                
                    <span v-if="selectedId!=-1">{{ selectedItem?.Name }}</span>
                    <br>
                    <span v-if="selectedId!=-1">{{ selectedItem?.Description }}</span>
                </div>
                <br>
                <input :disabled="selectedId==-1" v-model="range" type="range" min="1" :max="selectedItem==undefined ? 1 : selectedItem?.Count" class="slider" id="myRange">
                <button :disabled="selectedId==-1" @click="sellItem">Sell {{ range }}</button>
            </aside>
            <div class="d-inline-flexZZ items">
                <div v-for="item in player.inventory" :key="item.Id" class="item p-1">
                    <div class="d-inline-flex flex-column">
                        <span @mouseover="hoverId = item.Id" @mouseleave="hoverId = -1"
                        @click="selectItem(item.Id)" :class="selectedId==item.Id ? 'selectedItem' : ''">{{item.Name}} x {{item.Count}}</span>
                        <span v-if="hoverId==item.Id" class="m-2">
                            {{item.Description}}
                        </span>
                    </div>
                </div>
            </div>
            
        </section>
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
    border:1px solid black;
}

.right {
  background-color: rgb(250, 250, 250);
  width: 200px;
  float: right;
}
</style>