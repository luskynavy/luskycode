<template>
    <Line ref="myChart" id="myChart" :data="data" :options="options" />
    <label>>{{ $t('PricesStartAt0') }} <input id="PricesStartAt0" type='checkbox' @click='changeChartYAxis(this);'></label>
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

    let xValues = [
        "16/07/2021", "02/11/2021", "01/08/2022", "19/08/2022", "23/09/2022", "05/11/2022", "27/07/2023",];
    let yValues = [
        2.29, 2.29, 2.45, 2.45, 2.45, 2.45, 2.45,];

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
                if (cb.checked) {
                    this.$myChart.options.scales.y = { min: 0 }
                }
                else {
                    this.$myChart.options.scales.y = {}
                }
                this.$myChart.update()
            }
        },
        data() {
            return {
                data: {
                    labels: xValues,
                    datasets: [{
                        fill: false,
                        lineTension: 0,
                        label: this.name,
                        backgroundColor: "rgba(0,0,255,1.0)",
                        borderColor: "rgba(0,0,255,0.1)",
                        data: yValues
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
    }
</script>

<style scoped>
</style>