<template>
    <div class="container">
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
                            <th>Price</th>
                            <th>DateReceipt</th>
                            <th>SourceName</th>
                            <th>SourceLine</th>
                            <th>FullData</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="product in post" :key="product.id">
                            <td>{{ product.id }} </td>
                            <td>{{ product.group }}</td>
                            <td>{{ product.name }}</td>
                            <td>{{ product.price }}</td>
                            <td>{{ formatDate(product.dateReceipt) }}</td>
                            <td>{{ product.sourceName }}</td>
                            <td>{{ product.sourceLine }}</td>
                            <td>{{ product.fullData }}</td>
                            <!-- `` (backtick) template literrals pour pouvoir utiliser ${} du js et ne pas que ça passe pour une expression régulière "/details" -->
                            <td> <router-link :to="`/details/${product.id}`" class="bi bi-info-circle" title="Details"></router-link></td>
                        </tr>
                    </tbody>
                </table>
            </div>
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

                fetch(baseUrl + 'Products')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json;
                        this.loading = false;
                        return;
                    });
            },
            formatDate(date) {
                var options = {
                    year: "numeric",
                    month: "2-digit",
                    day: "numeric"
                };
                var d = new Date(date.slice(0, 10))
                return d.toLocaleString(navigator.language ? navigator.language : navigator['userLanguage'], options)
            }
        },
    });
</script>