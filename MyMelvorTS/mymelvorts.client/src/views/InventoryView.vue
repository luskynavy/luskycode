<script setup lang="ts">
    import { ref } from 'vue'
    //import { Sortable } from "sortablejs-vue3"
    import type { SortableEvent } from "sortablejs"    
    import draggable from 'vuedraggable'
    import { player } from '../stores/player'
    import InventoryItemClass from "../classes/InventoryItemClass"
    import Swal from 'sweetalert2'


    const hoverId = ref(-1)
    const selectedId = ref(-1)
    const range = ref(1)
    var selectedItem :InventoryItemClass | undefined

    //Change selected infos
    function selectItem(itemId:number) {
        selectedId.value=itemId
        selectedItem = player.inventory.find(i => i.Id == itemId)
        range.value = 1
    }

    //Sell selected nb 'range' item
    function sellItem() {
        const message = range.value + " " + selectedItem?.Name

        Swal.fire({
        text: "Sell " + message + " ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        }).then((result) => {
            if (result.isConfirmed) {
                player.sellItem(selectedId.value, range.value)

                let itemRemaining = player.inventory.find(i => i.Id == selectedId.value)

                //Remove selection if all items sold
                if (itemRemaining == undefined) {
                    selectedId.value = -1
                    selectedItem = undefined
                    range.value = 1
                }
                //Range value can't be more than item count
                else if (range.value > itemRemaining.Count ) {
                    range.value = itemRemaining.Count
                }

                Swal.fire({
                    text: "You have sold " + message,
                    icon: "success"
                })
            }
        })
    }

    //Changed sorting within list
	function onUpdate(event : SortableEvent) {        
        if (event.oldIndex === undefined|| event.newIndex === undefined) {
            return
        }
        //Move item to new place in inventory
        const item = player.inventory.splice(event.oldIndex, 1)[0]
        player.inventory.splice(event.newIndex, 0, item)
	}
</script>

<template>
    <main>
        <span>Inventory.vue</span>
        <section>
            <aside class="right ms-3">
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
            <!--<Sortable
                class="items"
                :list="player.inventory"
                itemKey="Id"                
                @update="onUpdate"
                @unchoose="console.log('unchoose')">
                    <template #item="{element, }">
                        <div class="item p-1" :key="element.Id"
                            @click="selectItem(element.Id)" 
                            @mouseover="hoverId = element.Id" @mouseleave="hoverId = -1">
                                <div class="d-inline-flex flex-column">
                                    <span :class="selectedId==element.Id ? 'selectedItem' : ''">{{element.Name}} xx {{element.Count}}</span>
                                    <span v-if="hoverId==element.Id" class="m-2">
                                        {{element.Description}}
                                    </span>
                                </div>
                        </div>
                    </template>
            </Sortable>-->

            <draggable
                class="items"
                v-model="player.inventory" 
                item-key="Id"
                @updateZZZ="onUpdate">
                <template #item="{element}">
                    <div class="item p-1" :key="element.Id"
                            @click="selectItem(element.Id)" 
                            @mouseover="hoverId = element.Id" @mouseleave="hoverId = -1">
                                <div class="d-inline-flex flex-column">
                                    <span :class="selectedId==element.Id ? 'selectedItem' : ''">{{element.Name}} x {{element.Count}}</span>
                                    <span v-if="hoverId==element.Id" class="m-2">
                                        {{element.Description}}
                                    </span>
                                </div>
                        </div>
                </template>
            </draggable>
            
            <!-- <div class="items">
                <span v-for="element in player.inventory" :key="element.Id" class="item p-1"
                    @click="selectItem(element.Id)" 
                    @mouseover="hoverId = element.Id" @mouseleave="hoverId = -1">
                    <div class="d-inline-flex flex-column">
                        <span :class="selectedId==element.Id ? 'selectedItem' : ''">{{element.Name}} x {{element.Count}}</span>
                        <span v-if="hoverId==element.Id" class="m-2">
                            {{element.Description}}
                        </span>
                    </div>
                </span>
            </div> -->
            
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