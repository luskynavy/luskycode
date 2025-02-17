import { Component, Input } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ProductsService } from '../services/products.service';
import { ChartConfiguration, ChartOptions, ChartType } from "chart.js";
import 'chartjs-adapter-date-fns';

@Component({
  selector: 'app-product-price',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './product-price.component.html',
  styleUrl: './product-price.component.css'
})
export class ProductPriceComponent {
  loaded: boolean = false;

  @Input()  id! :number;
  @Input()  name! :string;

  xValues:[] = [];
  yValues:[] = [];

  constructor(private productsService: ProductsService) {}

  ngOnInit(): void {
    this.getProductPrices();
  }

  getProductPrices() {
    this.productsService.getProductPrices(this.id).subscribe((data: any) => {
      this.xValues = data.map((p: { dateReceipt: string | any[]; }) => p.dateReceipt.slice(0, 10));
      this.yValues = data.map((p: { price: number; }) => p.price);

      this.lineChartData.labels = this.xValues;
      this.lineChartData.datasets[0].data = this.yValues;
      this.lineChartData.datasets[0].label = this.name;

      this.loaded = true;
    });
  }

  public lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: this.xValues,
    datasets: [
      {
        fill: false,
        tension: 0,
        label: this.name,
        backgroundColor: "rgba(0,0,255,1.0)",
        borderColor: "rgba(0,0,255,0.1)",
        data: this.yValues
      }
    ]
  };
  public lineChartOptions: ChartOptions<'line'> = {
    responsive: false,
    scales: {
      x: {
        type: 'time'
      }
    }
  };
  public lineChartLegend = true;
}
