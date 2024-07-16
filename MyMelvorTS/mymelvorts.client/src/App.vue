<script setup lang="ts">
  //import HelloWorld from './components/HelloWorld.vue'
  //import TheWelcome from './components/TheWelcome.vue'
  import 'bootstrap/dist/css/bootstrap.css'

  import { player } from './stores/player'
</script>

<template>
  <main>
      <div class="d-inline-flex flex-row container-fluidZZ">
          <header class="wrapperZZ mb-2ZZ">
              <!--<img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />-->
              <!--<nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">-->
              <nav class="navbar bg-light sidebar fixed-leftZZ p-3">
                  <div class="navbar-nav">
                      <span class="navbar-brand"><b>MyMelvor</b></span>
                      <RouterLink class="nav-link" to="/">Home</RouterLink>
                      <RouterLink class="nav-link" to="/inventory">Inventory</RouterLink>
                      <RouterLink class="nav-link" to="/woodcutting">Woodcutting {{player.woodcuttingLevel}}</RouterLink>
                      <RouterLink class="nav-link" to="/fishing">Fishing {{player.fishingLevel}}</RouterLink>
                      <RouterLink class="nav-link" to="/cooking">Cooking {{player.cookingLevel}}</RouterLink>
                      <RouterLink class="nav-link" to="/tests">Tests</RouterLink>
                  </div>
              </nav>
          </header>

          <div class="inside p-3">
              <!--<span>App.vue</span>-->
              <!--<TheWelcome />-->
              <RouterView />
          </div>
      </div>
  </main>
</template>

<style scoped>
    .sidebar {
        position: fixed;
        top: 0px;
        left: 0px;
        width: 150px;
    }

    .inside {
        position: fixed;
        top: 0px;
        left: 150px;
    }

    header {
        line-height: 1.5;
    }

    @media (min-width: 1024px) {
        /*header {
            display: flex;
            place-items: left;
            padding-right: calc(var(--section-gap) / 2);
        }

            header .wrapper {
                display: flex;
                place-items: flex-start;
                flex-wrap: wrap;
            }*/
    }
</style>
