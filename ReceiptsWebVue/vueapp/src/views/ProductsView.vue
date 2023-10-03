<template>
    <div class="container">
        <div class="post">
            <div v-if="loading" class="loading">
                Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
            </div>

            <div v-if="post" class="content">
                <Form @submit="fetchData">
                    <div class="form-actions no-color">
                        <p>
                            Filter by group : <Field name="filterGroup" as="select" v-model="filterGroup" class="form-control">
                                <option value=""></option>
                                <option :value="filterGroupValue"
                                        v-for="filterGroupValue in filterGroupValues"
                                        :key="filterGroupValue.id">
                                    {{ filterGroupValue }}
                                </option>
                            </Field>
                        </p>
                        <p>
                            Find by name : <Field id="SearchStringAutocomplete" name="searchString" v-model="searchString" type="text" class="form-control" autocomplete="off" />
                        </p>
                        <p>
                            Sort by : <Field name="sort" as="select" class="form-control" v-model="sort">
                                <option value="Group">Group</option>
                                <option value="DateReceipt">DateReceipt</option>
                                <option value="Name">Name</option>
                            </Field>
                        </p>

                        <button type="submit" class="btn btn-default btn-lg" title="Search">
                            <i class="bi bi-search"></i>
                        </button>
                        <a class="btn btn-default btn-lg" @click="clear" title="Clear">
                            <i class="bi bi-eraser"></i>
                        </a>
                    </div>

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

                    Page Size : <Field name="pageSize" as="select" class="form-control" v-model="pageSize" @change="selectChange">
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="100">100</option>
                        <option value="100000">All</option>
                    </Field>
                </Form>
            </div>
        </div>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { Form, Field } from 'vee-validate';

    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log("baseUrl: " + baseUrl);

    export default defineComponent({
        data() {
            return {
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