<script setup>
    import WeatherForecast from './components/WeatherForecast.vue'
    import TheWelcome from './components/TheWelcome.vue'
    import 'bootstrap/dist/css/bootstrap.css'

    import { player } from './stores/player.js'
</script>

<template>
    <header>
        <!--<img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />-->
        <!--<nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">-->
        <nav class="navbar bg-light sidebar">
            <div class="container-fluid navbar-nav">
                <span class="navbar-brand"><b>MyMelvor</b></span>
                <RouterLink class="nav-link" to="/">Home</RouterLink>
                <RouterLink class="nav-link" to="/inventory">Inventory</RouterLink>
                <RouterLink class="nav-link" to="/woodcutting">Woodcutting {{player.woodcuttingLevel}}</RouterLink>
                <RouterLink class="nav-link" to="/cooking">Cooking {{player.cookingLevel}}</RouterLink>
                <RouterLink class="nav-link" to="/tests">Tests</RouterLink>
            </div>
        </nav>
    </header>

    <main>
        <!--<span>App.vue</span>-->
        <!--<TheWelcome />-->
        <RouterView />
    </main>
</template>

<style scoped>
    .sidebar {
        position: fixed;
        top: 0;
        left: 0;
    }

    header {
        line-height: 1.5;
    }

    .logo {
        display: block;
        margin: 0 auto 2rem;
    }

    @media (min-width: 1024px) {
        header {
            display: flex;
            place-items: center;
            padding-right: calc(var(--section-gap) / 2);
        }

        .logo {
            margin: 0 2rem 0 0;
        }

        header .wrapper {
            display: flex;
            place-items: flex-start;
            flex-wrap: wrap;
        }
    }
</style>