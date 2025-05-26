import {Component, Input, OnInit, SimpleChanges} from '@angular/core';
import { Chart, ChartConfiguration, LineController, LineElement, PointElement, LinearScale, CategoryScale, Title, Tooltip, Legend } from 'chart.js';
import { DiscountDeltaResponse } from '../../interfaces/discount-delta-response';
import { BaseChartDirective } from 'ng2-charts';

Chart.register(
  LineController,
  LineElement,
  PointElement,
  LinearScale,
  CategoryScale,
  Title,
  Tooltip,
  Legend
);

@Component({
  selector: 'app-discount-chart',
  standalone: true,
  imports: [
    BaseChartDirective
  ],
  templateUrl: './discount-chart.component.html'
})
export class DiscountChartComponent implements OnInit {
  @Input() data: DiscountDeltaResponse[] = [];
  @Input() chartTitle: string = '';
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chartTitle'] && this.lineChartOptions?.plugins?.title) {
      this.lineChartOptions.plugins.title.text = this.chartTitle;
    }
  }

  public lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Відносна зміна доходу IT-компанія (%)',
        borderColor: '#42A5F5',
        backgroundColor: 'rgba(66, 165, 245, 0.2)',
        fill: true,
        tension: 0.4,
        pointBackgroundColor: 'white',
        pointBorderColor: '#42A5F5',
        pointRadius: 5,
        pointHoverRadius: 7,
        pointBorderWidth: 2
      },
      {
        data: [],
        label: 'Відносна зміна доходів провайдерів (%)',
        borderColor: '#66BB6A',
        backgroundColor: 'rgba(102, 187, 106, 0.2)',
        fill: true,
        tension: 0.4,
        pointBackgroundColor: 'white',
        pointBorderColor: '#66BB6A',
        pointRadius: 5,
        pointHoverRadius: 7,
        pointBorderWidth: 2
      },
      {
        data: [],
        label: 'Сумарна відносна зміна (%)',
        borderColor: '#FFA726',
        backgroundColor: 'rgba(255, 167, 38, 0.2)',
        fill: true,
        tension: 0.4,
        pointBackgroundColor: 'white',
        pointBorderColor: '#FFA726',
        pointRadius: 5,
        pointHoverRadius: 7,
        pointBorderWidth: 2
      }
    ]
  };


  public lineChartOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    animation: {
      duration: 1000,
      easing: 'easeInOutQuart'
    },
    plugins: {
      legend: { display: true },
      tooltip: {
        enabled: true,
        callbacks: {
          label: (context) =>
            `${context.dataset.label}: ${context.parsed.y.toFixed(2)}%`
        }
      },
      title: {
        display: true,
        text: this.chartTitle,
        font: {
          size: 18
        }
      }
    },
    scales: {
      x: {
        title: {
          display: true,
          text: 'Рівень знижки',
          font: {
            size: 16,
            weight: 'bold'
          }
        }
      },
      y: {
        title: {
          display: true,
          text: 'Зміна доходу (%)',
          font: {
            size: 16,
            weight: 'bold'
          }
        }
      }
    }
  };


  public lineChartType: 'line' = 'line';

  ngOnInit(): void {
    if (this.data?.length) {
      this.lineChartData.labels = this.data.map(d => `${(d.discount * 100).toFixed(0)}%`);
      this.lineChartData.datasets[0].data = this.data.map(d => d.companyIncomeDeltaPercent);
      this.lineChartData.datasets[1].data = this.data.map(d => d.providersIncomeDeltaPercent);
      this.lineChartData.datasets[2].data = this.data.map(d => d.totalDeltaPercent);
    }
  }
}
