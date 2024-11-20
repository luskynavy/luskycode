<template>
    <div class="post">
        <div v-if="loading" class="loading">
            <i18n-t keypath="LoadingNoLink" tag="p" scope="global">
                <!-- LoadingNoLink without param {0} to prevent crash -->
            </i18n-t>
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="forecast in post" :key="forecast.date">
                        <td>{{ forecast.date }}</td>
                        <td>{{ forecast.temperatureC }}</td>
                        <td>{{ forecast.temperatureF }}</td>
                        <td>{{ forecast.summary }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    Autocomplete:
    <SimpleTypeahead id="SearchStringAutocomplete"
                     v-model="searchString"
                     :items="['Vue','Vue1','Vue2','Vue3','Vue4','Vue5','Vue6','Script','Com']"
                     :minInputLength="1">
    </SimpleTypeahead>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import SimpleTypeahead from 'vue3-simple-typeahead'
    import 'vue3-simple-typeahead/dist/vue3-simple-typeahead.css'

    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log("baseUrl: " + baseUrl);

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null,
                searchString: ""
            };
        },
        components: {
            SimpleTypeahead
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;

                fetch(baseUrl + 'weatherforecast')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json;
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>

<style scoped>
    /*Enlève le retour à la ligne avant le div du SimpleTypeahead */
    div#SearchStringAutocomplete_wrapper {
        display: inline;
    }
</style>