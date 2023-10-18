<template>
    <Line ref="myChart" id="myChart" :data="charData.data" :options="charData.options" />
    <label>{{ $t('PricesStartAt0') }} <input id="PricesStartAt0" type='checkbox' v-model="PricesStartAt0Check" @click='changeChartYAxis(PricesStartAt0Check);'></label>
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
                if (cb) {
                    this.minY = undefined
                }
                else {
                    this.minY = 0
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
                            y: { min: this.minY }
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
                minY: undefined,
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
                    this.xValues = this.values.map(p => p.dateReceipt.slice(0, 10));
                    this.yValues = this.values.map(p => p.price);
                    return;
                });
        },
    }
</script>

<style scoped>
</style>