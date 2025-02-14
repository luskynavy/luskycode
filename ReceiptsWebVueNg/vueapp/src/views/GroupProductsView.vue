<template>

    <div class="post">
        <v-dialog v-model="modalProductsPrices" modal :style="{ width: '50vw' }">
            <v-card>
                <v-card-title>{{$t('PricesHistory')}}</v-card-title>
                <v-card-text>
                    <ProductPrices :name="modalProductName" :id="modalProductId" />
                </v-card-text>
            </v-card>
        </v-dialog>

        <div v-if="loading" class="loading">
            <i18n-t keypath="LoadingNoLink" tag="p" scope="global">
                <!--<a :href="vueUrl">{{ $t('vueUrl') }}</a>-->
            </i18n-t>
        </div>

        <div v-if="post" class="content">
            <form @submit.prevent="">
                <div class="form-actions no-color">
                    <p>
                        <!--{{ $t('FilterByGroup') }} :
                        <select name="filterGroup" v-model="filterGroup" class="form-control" @change="onGroupChange()">
                            <option value=""></option>
                            <option :value="filterGroupValue"
                                    v-for="filterGroupValue in filterGroupValues"
                                    :key="filterGroupValue.id">
                                {{ filterGroupValue }}
                            </option>
                        </select>-->
                        <v-select :label="$t('FilterByGroup')" v-model="filterGroup" @update:modelValue="onGroupChange()"
                                  :items="filterGroupValues">
                        </v-select>
                    </p>
                    <p>
                        <!--{{ $t('FindByName') }} : <input id="SearchStringAutocomplete" name="searchString" v-model="searchString" type="text" class="form-control" autocomplete="off" />-->
                        <v-combobox :label="$t('FindByName')" v-model="searchString" :items="productsNames" variantZZ="solo-inverted"></v-combobox>
                    </p>
                    <p>
                        <!--{{ $t('SortBy') }} :
                        <select name="sort" class="form-control" v-model="sort">
                            <option value="Group">{{ $t('Group') }}</option>
                            <option value="PriceRatio">{{ $t('PriceRatio') }}</option>
                            <option value="PricesCount">{{ $t('PricesCount') }}</option>
                        </select>-->
                        <v-select :label="$t('SortBy')" v-model="sort"
                                  :items="[{title:$t('Group'),value:'Group'},{title:$t('PriceRatio'),value:'PriceRatio'},{title:$t('PricesCount'),value:'PricesCount'},{title:$t('MaxDate'),value:'MaxDate'}]">
                        </v-select>
                    </p>

                    <v-switch :label="$t('IncludeProductsWithOnlyOnePrice')" v-model="products1price"></v-switch>

                    <button class="btn btn-default btn-lg buttonAsLink" :title="$t('Search')" @click="submitChanges">
                        <i class="bi bi-search"></i>
                    </button>
                    <a class="btn btn-default btn-lg buttonAsLink" @click="init" :title="$t('Clear')">
                        <i class="bi bi-eraser"></i>
                    </a>
                </div>
                <table class="table alternateLines">
                    <thead>
                        <tr>
                            <th>{{ $t('Id') }}</th>
                            <th>{{ $t('Group') }}</th>
                            <th>{{ $t('Name') }}</th>
                            <th>{{ $t('Min') }}</th>
                            <th>{{ $t('Max') }}</th>
                            <th>{{ $t('PreviousPrice') }}</th>
                            <th>{{ $t('LastPrice') }}</th>
                            <th>{{ $t('LastPricePerKilo') }}</th>
                            <th>{{ $t('Tendency') }}</th>
                            <th>{{ $t('MinDate') }}</th>
                            <th>{{ $t('MaxDate') }}</th>
                            <th>{{ $t('PriceRatio') }}</th>
                            <th>{{ $t('PricesCount') }}</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="groupProduct in post.data" :key="groupProduct.id">
                            <td>{{ groupProduct.id }} </td>
                            <td class="buttonAsLink" @click="changeGroup(groupProduct.group)">{{ groupProduct.group }}</td>
                            <td>{{ groupProduct.name }}</td>
                            <td>{{ groupProduct.min }}</td>
                            <td>{{ groupProduct.max }}</td>
                            <td>{{ groupProduct.previousPrice }}</td>
                            <td>{{ groupProduct.lastPrice }}</td>
                            <td>{{ groupProduct.lastPricePerKilo }}</td>
                            <td>
                                <span v-if="groupProduct.lastPrice > groupProduct.previousPrice" style="color:red">&#x2197;</span>
                                <span v-else-if="groupProduct.lastPrice == groupProduct.previousPrice">=</span>
                                <span v-else style="color:green">&#x2198;</span>
                            </td>
                            <td>{{ formatDate(groupProduct.minDate) }}</td>
                            <td>{{ formatDate(groupProduct.maxDate) }}</td>
                            <td>{{ groupProduct.priceRatio.toFixed(2) }}</td>
                            <td>{{ groupProduct.pricesCount }}</td>
                            <!-- `` (backtick) template literrals pour pouvoir utiliser ${} du js et ne pas que �a passe pour une expression r�guli�re "/details" -->
                            <td>
                                <router-link :to="`/details/${groupProduct.id}`" class="bi bi-info-circle" :title="$t('Details')"></router-link>
                                &nbsp;
                                <button type="button" class="buttonAsLink bi bi-graph-up" :title="$t('Prices')" @click="showModalProductsPrices(groupProduct.name, groupProduct.id)"></button>
                            </td>
                        </tr>
                    </tbody>
                </table>

                {{post.pageIndex}} {{ $t('of') }} {{post.totalPages}}
                <a @click="if (this.pageNumber != 1) {this.pageNumber = 1; submitChanges()}"
                   v-bind:class="post.pageIndex == 1 ? 'isDisabled': 'buttonAsLink'">
                    {{ $t('First') }}
                </a>
                &nbsp;
                <a @click="if (post.hasPreviousPage) {this.pageNumber = post.pageIndex - 1; submitChanges()}"
                   v-bind:class="post.hasPreviousPage ? 'buttonAsLink': 'isDisabled'">
                    {{ $t('Previous') }}
                </a>
                &nbsp;
                <a @click="if (post.hasNextPage) {this.pageNumber = post.pageIndex + 1; submitChanges()}"
                   v-bind:class="post.hasNextPage ? 'buttonAsLink': 'isDisabled'">
                    {{ $t('Next') }}
                </a>
                &nbsp;
                <a @click="if (this.pageNumber != post.totalPages) {this.pageNumber = post.totalPages; submitChanges()}"
                   v-bind:class="post.pageIndex == post.totalPages ? 'isDisabled': 'buttonAsLink'">
                    {{ $t('Last') }}
                </a>

                {{ $t('PageSize') }} :
                <!--<select name="pageSize" class="form-control" v-model="pageSize" @change="submitChanges">
                    <option value="10">10</option>
                    <option value="20">20</option>
                    <option value="100">100</option>
                    <option value="100000">{{ $t('All') }}</option>
                </select>-->
                <v-select v-model="pageSize" @update:modelValue="changePageSize" class="w6"
                          :items="[{title:'10',value:'10'},{title:'20',value:'20'},{title:'100',value:'100'},{title:$t('All'),value:'100000'}]" />
            </form>

            <button id="down" @click="exportMiniExcel()">
                {{ $t('ExportWithMiniExcel') }}
            </button>
        </div>
    </div>
</template>

<script lang="js">
    import axios from 'axios';

    //import { VCombobox } from 'vuetify/lib';
    import VueCookies from 'vue-cookies'

    import ProductPrices from '../components/ProductPrices.vue';

    //imports are working but override all styles in all others views
    //import 'vuetify/styles'
    //import 'vuetify/dist/vuetify-labs.css';
    //import 'vuetify/lib/styles/main.css';

    const cookieName = 'DefaultGroupSort'
    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log('baseUrl: ' + baseUrl);

    let defaultSort = 'Group';
    let defaultPageSize = '10';

    export default {
        data() {
            return {
                loading: false,
                post: null,
                filterGroup: '',
                filterGroupValues: [],
                productsNames: [],
                searchString: '',
                sort: defaultSort,
                products1price: false,
                pageSize: defaultPageSize,
                pageNumber: 1,
                modalProductsPrices: false,
                modalProductname: '',
                modalProductId: 0,
                itemsSort: [
                    {
                        title: this.$t('Group'),
                        value: 'Group',
                    },
                    {
                        title: this.$t('PriceRatio'),
                        value: 'PriceRatio',
                    },
                    {
                        title: this.$t('PricesCount'),
                        value: 'PricesCount',
                    },
                ]
            };
        },
        components: {
            ProductPrices,
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

            this.init();

            document.title = this.$t('GroupProducts');
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            init() {
                let cookieDefaultSort = VueCookies.get(cookieName)
                if (cookieDefaultSort == null) {
                    cookieDefaultSort = 'Group'
                }

                this.filterGroup = '';
                this.searchString = '';
                this.sort = cookieDefaultSort;
                this.products1price = false;
                this.pageSize = defaultPageSize;
                this.pageNumber = 1;

                this.submitChanges();
            },
            submitChanges() {
                let values = {
                    filterGroup: this.filterGroup,
                    searchString: this.searchString,
                    sort: this.sort,
                    products1price: this.products1price,
                    pageSize: this.pageSize,
                    pageNumber: this.pageNumber
                }
                this.fetchData(values)
            },
            fetchData(values) {
                this.post = null;
                this.loading = true;

                /*if (values != undefined) {
                    console.log('filterGroup: ' + values.filterGroup);
                    console.log('searchString: ' + values.searchString);
                    console.log('sort: ' + values.sort);
                    console.log('products1price: ' + values.products1price);
                    console.log('pageSize: ' + values.pageSize);
                    console.log('pageNumber: ' + values.pageNumber);
                }*/

                if (values.sort !== undefined) {
                    VueCookies.set(cookieName, values.sort, '1y')
                }

                axios.get(baseUrl + 'GroupProducts',
                    { params: values })
                    .then(r => r.data)
                    .then(json => {
                        this.post = json;
                        this.loading = false;
                        return;
                    });

            },
            formatDate(date) {
                var options = {
                    year: 'numeric',
                    month: '2-digit',
                    day: 'numeric'
                };
                var d = new Date(date.slice(0, 10))
                return d.toLocaleString(navigator.language ? navigator.language : navigator['userLanguage'], options)
            },
            showModalProductsPrices(name, id) {
                this.modalProductsPrices = true
                this.modalProductName = name
                this.modalProductId = id
            },
            changeGroup(group) {
                this.filterGroup = group;

                this.onGroupChange();
                this.submitChanges();
            },
            onGroupChange() {
                //console.log("group change " + this.filterGroup)

                if (this.filterGroup != '') {
                    this.searchString = ''
                }

                fetch(baseUrl + 'ProductsNames?group=' + encodeURIComponent(this.filterGroup))
                    .then(r => r.json())
                    .then(json => {
                        this.productsNames = json;
                        return;
                    });
            },
            changePageSize() {
                this.pageNumber = 1
                this.submitChanges()
            },
            exportMiniExcel() {
                axios.get(baseUrl + 'ExportGroupProductsMiniExcel', { responseType: 'blob' })
                    .then((response) => {
                        const url = window.URL.createObjectURL(new Blob([response.data]));
                        const link = document.createElement('a');
                        link.href = url;
                        link.setAttribute('download', 'ExportGroupMiniExcel.xlsx');
                        document.body.appendChild(link);
                        link.click();
                        URL.revokeObjectURL(link.href)
                    })
            },
        },
    }
</script>

<!--<style lang="css" scoped src="vuetify/dist/vuetify-labs.css">
</style>-->
<!--do nothing-->
<style lang="css" scoped src="vuetify/lib/styles/main.css">
    /*do nothing if src used*/
    .w6 {
        width: 6rem;
    }
</style>