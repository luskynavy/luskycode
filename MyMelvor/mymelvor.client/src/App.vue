<script setup>
    import 'bootstrap/dist/css/bootstrap.css'

    //import { player } from './stores/player'
    import { usePlayerStore } from './stores/player'

    const player = usePlayerStore()
</script>

<template>
    <main>
        <div>
            <header>
                <nav class="navbar bg-light sidebar">
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

            <div class="inside">
                <RouterView />
            </div>
        </div>
    </main>
</template>

<style scoped>
    .sidebar {
        position: fixed;
        /*top: 0px;*/
        left: 20px;
        width: 150px;
    }

    .inside {
        position: relative;
        /*top: 0px;*/
        left: 150px;
    }

    header {
        line-height: 1.5;
    }

    @media (min-width: 512px) {
    }
</style>