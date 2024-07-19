<script setup lang="ts">
  import 'bootstrap/dist/css/bootstrap.css'

  import { player } from './stores/player'
</script>

<template>
  <main>
      <div class="d-inline-flex flex-row">
          <header >
              <nav class="navbar bg-light sidebar p-3">
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
    }
</style>
