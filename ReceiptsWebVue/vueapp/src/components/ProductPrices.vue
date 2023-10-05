<template>
    <Line ref="myChart" id="myChart" :data="charData.data" :options="charData.options" />
    <label>>{{ $t('PricesStartAt0') }} <input id="PricesStartAt0" type='checkbox' v-model="PricesStartAt0Check" @click='changeChartYAxis(PricesStartAt0Check);'></label>
</template>

<script>
    import {
        Chart as ChartJS,
        CategoryScale,
        LinearScale,
        PointElement,
        LineElement,
        Title,
        Tooltip,
        Legend
    } from 'chart.js'
    import { Line } from 'vue-chartjs'

    const baseUrl = `${import.meta.env.VITE_API_URL}`;
    //console.log("baseUrl: " + baseUrl);

    ChartJS.register(
        CategoryScale,
        LinearScale,
        PointElement,
        LineElement,
        Title,
        Tooltip,
        Legend
    )

    export default {
        name: 'ProductPrices',
        props: {
            name: String,
            id: Number,
        },
        components: {
            Line
        },
        methods: {
            changeChartYAxis(cb) {
                //to check (use computed for myChart.options) https://stackoverflow.com/questions/76234815/vue-chart-js-when-i-add-data-charts-are-not-updated/
                if (cb) {
                    this.charData.options.scales.y = { min: 0 }
                }
                else {
                    this.charData.options.scales.y = {}
                }
            }
        },
        computed: {
            charData() {
                return {
                    data: {
                        labels: this.xValues,
                        datasets: [{
                            fill: false,
                            lineTension: 0,
                            label: this.name,
                            backgroundColor: "rgba(0,0,255,1.0)",
                            borderColor: "rgba(0,0,255,0.1)",
                            data: this.yValues
                        }]
                    },
                    options: {
                        plugins: {
                            legend: { display: true },
                        },
                        scales: {
                            //y: { min: 0 }
                        }
                    }
                }
            }
        },
        data() {
            return {
                values: [],
                xValues: [],
                yValues: [],
                PricesStartAt0Check: false
            }
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            fetch(baseUrl + 'GetProductPrices?id=' + this.id)
                .then(r => r.json())
                .then(json => {
                    this.values = json;
                    this.xValues = this.values.map(p => p.dateReceipt);
                    this.yValues = this.values.map(p => p.price);
                    return;
                });
        },
    }
</script>

<style scoped>
</style>