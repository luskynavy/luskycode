<template>
    <div class="container">
        <main role="main" class="pb-3">
            <div class="post">
                <div v-if="loading" class="loading">
                    <i18n-t keypath="LoadingNoLink" tag="p" scope="global">
                        <!-- LoadingNoLink without param {0} to prevent crash -->
                        <!--<a :href="vueUrl">{{ $t('vueUrl') }}</a>-->
                    </i18n-t>
                </div>

                <div v-if="post" class="content">
                    <form @submit.prevent="">
                        <div class="form-actions no-color">
                            <p>
                                {{ $t('FilterByGroup') }} :
                                <select name="filterGroup" v-model="filterGroup" class="form-control">
                                    <option value=""></option>
                                    <option :value="filterGroupValue"
                                            v-for="filterGroupValue in filterGroupValues"
                                            :key="filterGroupValue.id">
                                        {{ filterGroupValue }}
                                    </option>
                                </select>
                            </p>
                            <p>
                                {{ $t('FindByName') }} : <!--<SimpleTypeahead id="SearchStringAutocomplete" v-model="searchString" :items="productsNames" :minInputLength="1" autocomplete="off" />-->
                                <input id="SearchStringAutocomplete" name="searchString" v-model="searchString" type="text" class="form-control" autocomplete="off" />
                            </p>
                            <p>
                                {{ $t('SortBy') }} :
                                <select name="sort" class="form-control" v-model="sort">
                                    <option value="Group">{{ $t('Group') }}</option>
                                    <option value="DateReceipt">{{ $t('DateReceipt') }}</option>
                                    <option value="Name">{{ $t('Name') }}</option>
                                </select>
                            </p>

                            <button class="btn btn-default btn-lg" :title="$t('Search')" @click="submitChanges">
                                <i class="bi bi-search"></i>
                            </button>
                            <a class="btn btn-default btn-lg" @click="clear" :title="$t('Clear')">
                                <i class="bi bi-eraser"></i>
                            </a>
                        </div>

                        <table class="table alternateLines">
                            <thead>
                                <tr>
                                    <th>{{ $t('Id') }}</th>
                                    <th>{{ $t('Group') }}</th>
                                    <th>{{ $t('Name') }}</th>
                                    <th>{{ $t('Price') }}</th>
                                    <th>{{ $t('DateReceipt') }}</th>
                                    <th>{{ $t('SourceName') }}</th>
                                    <th>{{ $t('SourceLine') }}</th>
                                    <th>{{ $t('FullData') }}</th>
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
                                    <td> <router-link :to="`/details/${product.id}`" class="bi bi-info-circle" :title="$t('Details')"></router-link></td>
                                </tr>
                            </tbody>
                        </table>

                        {{ $t('PageSize') }} :
                        <select name="pageSize" class="form-control" v-model="pageSize" @change="submitChanges">
                            <option value="10">10</option>
                            <option value="20">20</option>
                            <option value="100">100</option>
                            <option value="100000">{{ $t('All') }}</option>
                        </select>
                    </form>
                </div>
            </div>
        </main>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import SimpleTypeahead from 'vue3-simple-typeahead'
    import 'vue3-simple-typeahead/dist/vue3-simple-typeahead.css'

    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log("baseUrl: " + baseUrl);

    let defaultSort = "Group";
    let defaultPageSize = 10;

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null,
                filterGroup: "",
                filterGroupValues: [],
                productsNames: [],
                searchString: "",
                sort: defaultSort,
                pageSize: defaultPageSize
            };
        },
        components: {
            SimpleTypeahead
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            fetch(baseUrl + 'GroupSelectList')
                .then(r => r.json())
                .then(json => {
                    this.filterGroupValues = json;
                    return;
                });

            fetch(baseUrl + 'ProductsNames?search=')
                .then(r => r.json())
                .then(json => {
                    this.productsNames = json;
                    return;
                });

            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            clear() {
                this.filterGroup = "";
                this.searchString = "";
                this.sort = defaultSort;
                this.pageSize = defaultPageSize;

                this.fetchData();
            },
            submitChanges() {
                let values = {
                    filterGroup: encodeURIComponent(this.filterGroup),
                    searchString: encodeURIComponent(this.searchString),
                    sort: encodeURIComponent(this.sort),
                    pageSize: encodeURIComponent(this.pageSize)
                }
                this.fetchData(values)
            },
            fetchData(values) {
                this.post = null;
                this.loading = true;

                let params = ''

                /*if (values != undefined) {
                    console.log("filterGroup: " + values.filterGroup);
                    console.log("searchString: " + values.searchString);
                    console.log("sort: " + values.sort);
                    console.log("pageSize: " + values.pageSize);
                }*/

                if (values !== undefined) {
                    params += '?'
                    params += 'filterGroup=' + (values.filterGroup !== undefined ? values.filterGroup : '')
                    params += '&searchString=' + (values.searchString !== undefined ? values.searchString : '')
                    params += '&sort=' + (values.sort !== undefined ? values.sort : '')
                    params += '&pageSize=' + (values.pageSize !== undefined ? values.pageSize : '10')
                    params += '&pageNumber=' + (values.pageNumber !== undefined ? values.pageNumber : '')
                }
                //console.log("params: " + params);

                fetch(baseUrl + 'Products' + params)
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

<style scoped>
    /*Enlève le retour à la ligne avant le div du SimpleTypeahead */
    div#SearchStringAutocomplete_wrapper {
        display: inline;
    }
</style>