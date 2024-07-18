<script setup>
    //import WeatherForecast from './components/WeatherForecast.vue'
    //import TheWelcome from './components/TheWelcome.vue'
    import 'bootstrap/dist/css/bootstrap.css'

    //import { player } from './stores/player'
    import { usePlayerStore } from './stores/player'

    const player = usePlayerStore()
</script>

<template>
    <main>
        <div class="container-fluidZZ">
            <header class="wrapperZZ">
                <!--<img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />-->
                <!--<nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">-->
                <nav class="navbar bg-light sidebar fixed-leftZZ">
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