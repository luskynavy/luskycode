<template>
    <div class="container">
        <main role="main" class="pb-3">
            <Dialog v-model:visible="modalProductsPrices" modal :header="$t('PricesHistory')" :style="{ width: '50vw' }">
                <p>
                    <ProductPrices :name="modalProductName" :id="modalProductId" />
                </p>
            </Dialog>

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
                                <!--<select name="filterGroup" v-model="filterGroup" class="form-control" @change="onGroupChange()">
                                    <option value=""></option>
                                    <option :value="filterGroupValue"
                                            v-for="filterGroupValue in filterGroupValues"
                                            :key="filterGroupValue.id">
                                        {{ filterGroupValue }}
                                    </option>
                                </select>-->
                                <Dropdown v-model="filterGroup" @change="onGroupChange()"
                                          :options="filterGroupValues" />
                            </p>
                            <p>
                                {{ $t('FindByName') }} :
                                <AutoComplete v-model="searchString" dropdown :emptySearchMessage="$t('NoResultsFound')" :suggestions="filteredProducts" @complete="searchProduct"></AutoComplete>
                                <!--<input id="SearchStringAutocomplete" name="searchString" v-model="searchString" type="text" class="form-control" autocomplete="off" />-->
                            </p>
                            <p>
                                {{ $t('SortBy') }} :
                                <!--<select name="sort" class="form-control" v-model="sort">
                                    <option value="Group">{{ $t('Group') }}</option>
                                    <option value="DateReceipt">{{ $t('DateReceipt') }}</option>
                                    <option value="Name">{{ $t('Name') }}</option>
                                </select>-->
                                <Dropdown v-model="sort" optionLabel="title" optionValue="value"
                                          :options="[{title:$t('Group'),value:'Group'},{title:$t('DateReceipt'),value:'DateReceipt'},{title:$t('Name'),value:'Name'}]" />
                            </p>

                            <button class="btn btn-default btn-lg buttonAsLink" :title="$t('Search')" @click="submitChanges">
                                <i class="bi bi-search"></i>
                            </button>
                            <a class="btn btn-default btn-lg buttonAsLink" @click="init" :title="$t('Clear')">
                                <i class="bi bi-eraser"></i>
                            </a>
                        </div>

                        <table class="table alternateLinesZZZ">
                            <thead>
                                <tr>
                                    <th>{{ $t('Id') }}</th>
                                    <th>{{ $t('Group') }}</th>
                                    <th>{{ $t('Name') }}</th>
                                    <th>{{ $t('Price') }}</th>
                                    <th>{{ $t('PricePerKilo') }}</th>
                                    <th>{{ $t('DateReceipt') }}</th>
                                    <th>{{ $t('SourceName') }}</th>
                                    <th>{{ $t('SourceLine') }}</th>
                                    <th>{{ $t('FullData') }}</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="product in post.data" :key="product.id" class="even:bg-grey">
                                    <td>{{ product.id }} </td>
                                    <td class="buttonAsLink" @click="changeGroup(product.group)">{{ product.group }}</td>
                                    <td>{{ product.name }}</td>
                                    <td>{{ product.price }}</td>
                                    <td>{{ product.pricePerKilo }}</td>
                                    <td>{{ formatDate(product.dateReceipt) }}</td>
                                    <td>{{ product.sourceName }}</td>
                                    <td>{{ product.sourceLine }}</td>
                                    <td>{{ product.fullData }}</td>
                                    <!-- `` (backtick) template literrals pour pouvoir utiliser ${} du js et ne pas que �a passe pour une expression r�guli�re "/details" -->
                                    <td>
                                        <router-link :to="`/details/${product.id}`" class="bi bi-info-circle" :title="$t('Details')"></router-link>
                                        &nbsp;
                                        <button type="button" class="buttonAsLink bi bi-graph-up" :title="$t('Prices')" @click="showModalProductsPrices(product.name, product.id)"></button>
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
                        <Dropdown v-model="pageSize" @change="changePageSize" optionLabel="title" optionValue="value"
                                  :options="[{title:'10',value:'10'},{title:'20',value:'20'},{title:'100',value:'100'},{title:$t('All'),value:'100000'}]" />
                    </form>

                    <!--<button id="down" @click="ExportMiniExcel()">
                        {{ $t('ExportWithMiniExcel') }}
                    </button>-->

                    <a :href="linkExport">{{ $t('ExportWithMiniExcel') }}</a>
                </div>
            </div>
        </main>
    </div>
</template>

<script lang="js">
    import { ref } from 'vue';

    import Dialog from 'primevue/dialog';
    import Dropdown from 'primevue/dropdown';
    import AutoComplete from 'primevue/autocomplete'
    import 'primevue/resources/themes/bootstrap4-light-blue/theme.css';

    import VueCookies from 'vue-cookies'

    import ProductPrices from '../components/ProductPrices.vue';

    const cookieName = 'DefaultSort'
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
                filteredProducts: ref(),
                productsNames: [],
                searchString: '',
                sort: defaultSort,
                pageSize: defaultPageSize,
                pageNumber: 1,
                modalProductsPrices: false,
                modalProductname: '',
                modalProductId: 0,
                linkExport: baseUrl + 'ExportProductsMiniExcel'
            };
        },
        components: {
            ProductPrices,
            Dialog,
            Dropdown,
            AutoComplete
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
                this.pageSize = defaultPageSize;
                this.pageNumber = 1;

                this.submitChanges();
            },
            submitChanges() {
                let values = {
                    filterGroup: encodeURIComponent(this.filterGroup),
                    searchString: encodeURIComponent(this.searchString),
                    sort: encodeURIComponent(this.sort),
                    pageSize: encodeURIComponent(this.pageSize),
                    pageNumber: encodeURIComponent(this.pageNumber),
                }
                this.fetchData(values)
            },
            fetchData(values) {
                this.post = null;
                this.loading = true;

                let params = ''

                /*if (values != undefined) {
                    console.log('filterGroup: ' + values.filterGroup);
                    console.log('searchString: ' + values.searchString);
                    console.log('sort: ' + values.sort);
                    console.log('pageSize: ' + values.pageSize);
                    console.log('pageNumber: ' + values.pageNumber);
                }*/

                if (values !== undefined) {
                    params += '?'
                    params += 'filterGroup=' + (values.filterGroup !== undefined ? values.filterGroup : '')
                    params += '&searchString=' + (values.searchString !== undefined ? values.searchString : '')
                    params += '&sort=' + (values.sort !== undefined ? values.sort : '')
                    params += '&pageSize=' + (values.pageSize !== undefined ? values.pageSize : '10')
                    params += '&pageNumber=' + (values.pageNumber !== undefined ? values.pageNumber : '')

                    if (values.sort !== undefined) {
                        VueCookies.set(cookieName, values.sort, '1y')
                    }
                }
                //console.log('params: ' + params);

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
                    year: 'numeric',
                    month: '2-digit',
                    day: 'numeric'
                };
                var d = new Date(date.slice(0, 10))
                return d.toLocaleString(navigator.language ? navigator.language : navigator['userLanguage'], options)
            },
            searchProduct(event) {
                this.filteredProducts = this.productsNames.filter((product) => {
                    return product.toLowerCase().includes(event.query.toLowerCase());
                });
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
            /*exportMiniExcel() {
                fetch(baseUrl + 'ExportProductsMiniExcel')
                    .then(r => r.blob())
                    .then(json => {
                        const url = window.URL.createObjectURL(new Blob([json]));
                        const link = document.createElement('a');
                        link.href = url;
                        link.setAttribute('download', 'ExportMiniExcel.xlsx');
                        document.body.appendChild(link);
                        link.click();
                        return;
                    });
            },*/
        },
    };
</script>

<style scoped>
</style>