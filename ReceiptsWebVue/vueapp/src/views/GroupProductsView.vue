<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table class="table alternateLines">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Group</th>
                        <th>Name</th>
                        <th>Min</th>
                        <th>Max</th>
                        <th>MinDate</th>
                        <th>MaxDate</th>
                        <th>PriceRatio</th>
                        <th>PricesCount</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="groupProduct in post" :key="groupProduct.id">
                        <td>{{ groupProduct.id }} </td>
                        <td>{{ groupProduct.group }}</td>
                        <td>{{ groupProduct.name }}</td>
                        <td>{{ groupProduct.min }}</td>
                        <td>{{ groupProduct.max }}</td>
                        <td>{{ groupProduct.minDate.slice(0, 10) }}</td>
                        <td>{{ groupProduct.maxDate.slice(0, 10) }}</td>
                        <td>{{ groupProduct.priceRatio.toFixed(2) }}</td>
                        <td>{{ groupProduct.pricesCount }}</td>
                        <!-- `` (backtick) template literrals pour pouvoir utiliser ${} du js et ne pas que ça passe pour une expression régulière "/details" -->
                        <td> <router-link :to="`/details/${groupProduct.id}`" class="bi bi-info-circle" title="Details"></router-link></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';

    const baseUrl = `${import.meta.env.VITE_API_URL}`;

    //console.log("baseUrl: " + baseUrl);

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null
            };
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

                fetch(baseUrl + 'GroupProducts')
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