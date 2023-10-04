<template>
    <div class="post">
        <div v-if="loading" class="loading">
            <i18n-t keypath="Loading" tag="p" scope="global">
                <a :href="vueUrl">{{ $t('vueUrl') }}</a>
            </i18n-t>
        </div>

        <div v-if="post" class="content">
            <Form @submit="fetchData">
                <div class="form-actions no-color">
                    <p>
                        {{ $t('FilterByGroup') }} : <Field name="filterGroup" as="select" v-model="filterGroup" class="form-control">
                            <option value=""></option>
                            <option :value="filterGroupValue"
                                    v-for="filterGroupValue in filterGroupValues"
                                    :key="filterGroupValue.id">
                                {{ filterGroupValue }}
                            </option>
                        </Field>
                    </p>
                    <p>
                        {{ $t('FindByName') }} : <Field id="SearchStringAutocomplete" name="searchString" v-model="searchString" type="text" class="form-control" autocomplete="off" />
                    </p>
                    <p>
                        {{ $t('SortBy') }} : <Field name="sort" as="select" class="form-control" v-model="sort">
                            <option value="Group">{{ $t('Group') }}</option>
                            <option value="PriceRatio">{{ $t('PriceRatio') }}</option>
                            <option value="PricesCount">{{ $t('PricesCount') }}</option>
                        </Field>
                    </p>

                    <button type="submit" class="btn btn-default btn-lg" :title="$t('Search')">
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
                            <th>{{ $t('Min') }}</th>
                            <th>{{ $t('Max') }}</th>
                            <th>{{ $t('MinDate') }}</th>
                            <th>{{ $t('MaxDate') }}</th>
                            <th>{{ $t('PriceRatio') }}</th>
                            <th>{{ $t('PricesCount') }}</th>
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
                            <td>{{ formatDate(groupProduct.minDate) }}</td>
                            <td>{{ formatDate(groupProduct.maxDate) }}</td>
                            <td>{{ groupProduct.priceRatio.toFixed(2) }}</td>
                            <td>{{ groupProduct.pricesCount }}</td>
                            <!-- `` (backtick) template literrals pour pouvoir utiliser ${} du js et ne pas que ça passe pour une expression régulière "/details" -->
                            <td> <router-link :to="`/details/${groupProduct.id}`" class="bi bi-info-circle" :title="$t('Details')"></router-link></td>
                        </tr>
                    </tbody>
                </table>

                {{ $t('PageSize') }} : <Field name="pageSize" as="select" class="form-control" v-model="pageSize" @change="selectChange">
                    <option value="10">10</option>
                    <option value="20">20</option>
                    <option value="100">100</option>
                    <option value="100000">{{ $t('All') }}</option>
                </Field>
            </Form>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { Form, Field } from 'vee-validate';
    import axios from "axios";

    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log("baseUrl: " + baseUrl);

    export default defineComponent({
        data() {
            return {
                vueUrl: 'https://aka.ms/jspsintegrationvue',
                loading: false,
                post: null,
                filterGroup: "",
                filterGroupValues: [],
                searchString: "",
                sort: "Group",
                pageSize: 10
            };
        },
        components: {
            Form,
            Field
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
                this.sort = "Group";
                this.pageSize = 10;

                this.fetchData();
            },
            selectChange() {
                let values = {
                    filterGroup: this.filterGroup,
                    searchString: this.searchString,
                    sort: this.sort,
                    pageSize: this.pageSize
                }
                this.fetchData(values)
            },
            fetchData(values) {
                this.post = null;
                this.loading = true;

                /*if (values != undefined) {
                    console.log("filterGroup: " + values.filterGroup);
                    console.log("searchString: " + values.searchString);
                    console.log("sort: " + values.sort);
                    console.log("pageSize: " + values.pageSize);
                }*/

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