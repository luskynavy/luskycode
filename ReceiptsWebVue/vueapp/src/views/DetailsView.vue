<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <dl class="row">
                <dt class="col-sm-2">
                    Id
                </dt>
                <dd class="col-sm-10">
                    {{ post.id }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    Name
                </dt>
                <dd class="col-sm-10">
                    {{ post.name }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    Group
                </dt>
                <dd class="col-sm-10">
                    {{ post.group }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    Price
                </dt>
                <dd class="col-sm-10">
                    {{ post.price }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    DateReceiptd
                </dt>
                <dd class="col-sm-10">
                    {{ post.dateReceipt }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    SourceName
                </dt>
                <dd class="col-sm-10">
                    {{ post.sourceName }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    SourceLine
                </dt>
                <dd class="col-sm-10">
                    {{ post.sourceLine }}
                </dd>
            </dl>
            <dl class="row">
                <dt class="col-sm-2">
                    FullData
                </dt>
                <dd class="col-sm-10">
                    {{ post.fullData }}
                </dd>
            </dl>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { useRoute } from 'vue-router';

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

                const route = useRoute();
                const id = route.params.id;

                console.log("id: " + id);

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