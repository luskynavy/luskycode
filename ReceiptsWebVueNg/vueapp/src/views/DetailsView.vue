<template>
    <div class="post">
        <div v-if="loading" class="loading">
            <i18n-t keypath="LoadingNoLink" tag="p" scope="global">
            </i18n-t>
        </div>

        <div v-if="post" class="content">
            <dl class="row">
                <dt class="col-sm-2">
                    {{ $t('Id') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.id }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('Name') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.name }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('Group') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.group }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('Price') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.price }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('DateReceipt') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.dateReceipt }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('SourceName') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.sourceName }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('SourceLine') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.sourceLine }}
                </dd>
                <dt class="col-sm-2">
                    {{ $t('FullData') }}
                </dt>
                <dd class="col-sm-10">
                    {{ post.fullData }}
                </dd>
            </dl>

            <ProductPrices :name="post.name" :id="post.id" />
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { useRoute } from 'vue-router';
    import ProductPrices from '../components/ProductPrices.vue';

    //import 'bootstrap/dist/css/bootstrap.css'

    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log("baseUrl: " + baseUrl);

    export default defineComponent({
        data() {
            return {
                vueUrl: 'https://aka.ms/jspsintegrationvue',
                loading: false,
                post: null
            };
        },
        components: {
            ProductPrices
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();

            document.title = this.$t('Details');
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;

                const route = useRoute();
                const id = route.params.id;

                //console.log("id: " + id);

                fetch(baseUrl + 'Products/' + id)
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

<!--For dl/dt/dd css of bootstrap-->
<style lang="css" scoped src="bootstrap/dist/css/bootstrap.css">
</style>