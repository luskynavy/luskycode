<script setup>
    import { ref } from 'vue'
    //import { player } from '../stores/player'
    import { usePlayerStore } from '../stores/player'
    import Swal from 'sweetalert2'

    const player = usePlayerStore()
    const hoverId = ref(-1)
    const selectedId = ref(-1)
    const range = ref(1)
    var selectedItem = undefined

    function selectItem(itemId) {
        selectedId.value=itemId
        selectedItem = player.inventory.find(i => i.Id == itemId)
        range.value = 1
    }

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
                <div v-for="item in player.inventory" :key="item.Id" class="item p-1"
                    @click="selectItem(item.Id)" 
                    @mouseover="hoverId = item.Id" @mouseleave="hoverId = -1">
                    <div class="d-inline-flex flex-column">
                        <span :class="selectedId==item.Id ? 'selectedItem' : ''">{{item.Name}} x {{item.Count}}</span>
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