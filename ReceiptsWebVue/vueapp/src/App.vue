<script setup>
    import { RouterLink, RouterView } from 'vue-router'
    //import HelloWorld from './components/HelloWorld.vue'
    import LocaleSwitcher from './components/l10n/LocaleSwitcher.vue'
</script>

<template>
    <header>
        <!--<img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />-->

        <div class="wrapperXXX">
            <!--<HelloWorld msg="ReceiptsWebVue" />-->

            <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div class="container-fluid">
                    <span class="navbar-brand"><b>ReceiptsWebVue</b></span>
                    <!--<RouterLink to="/">Home</RouterLink>
                    <RouterLink to="/about">About</RouterLink>-->
                    <RouterLink to="/groupproducts">{{ $t('GroupProducts') }}</RouterLink>
                    <RouterLink to="/products">{{ $t('Products') }}</RouterLink>
                    <RouterLink to="/weather">{{ $t('Tests') }}</RouterLink>

                    <LocaleSwitcher class="floatRight" />
                </div>
            </nav>
        </div>
    </header>

    <RouterView />
</template>

<style scoped>
    header {
        line-height: 1.5;
        max-height: 100vh;
    }

    .floatRight {
        float: right
    }

    .navbar-brand {
        white-space: normal;
        text-align: center;
        word-break: break-all;
    }

    .logo {
        display: block;
        margin: 0 auto 2rem;
    }

    nav {
        width: 100%;
        font-size: 12px;
        text-align: center;
        margin-top: 2rem;
    }

        nav a.router-link-exact-active {
            color: var(--color-text);
        }

            nav a.router-link-exact-active:hover {
                background-color: transparent;
            }

        nav a {
            display: inline-block;
            padding: 0 1rem;
            border-left: 1px solid var(--color-border);
        }

            nav a:first-of-type {
                border: 0;
            }

    @media (min-width: 1024px) {
        header {
            /*display: flex;*/
            place-items: center;
            padding-right: calc(var(--section-gap) / 2);
        }

        .logo {
            margin: 0 2rem 0 0;
        }

        header .wrapper {
            /*display: flex;*/
            place-items: flex-start;
            flex-wrap: wrap;
        }

        nav {
            text-align: left;
            margin-left: -1rem;
            font-size: 1rem;
            padding: 1rem 0;
            margin-top: 1rem;
        }
    }
</style>