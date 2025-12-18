import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-admin-sales-stats',
  standalone: false,
  templateUrl: './sales-stats.component.html',
  styleUrls: ['./sales-stats.component.scss'],
})
export class AdminSalesStatsComponent implements AfterViewInit {

  @ViewChild('revenueTrendCanvas') revenueTrendCanvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('salesByMakeCanvas') salesByMakeCanvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('salesByTypeCanvas') salesByTypeCanvas!: ElementRef<HTMLCanvasElement>;

  ngAfterViewInit(): void {
    this.initRevenueTrend();
    this.initSalesByMake();
    this.initSalesByType();
  }

  private initRevenueTrend() {
    const cfg: ChartConfiguration<'line'> = {
      type: 'line',
      data: {
        labels: ['Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        datasets: [{
          label: 'Revenue',
          data: [320, 280, 340, 390, 360, 425],
          tension: 0.35,
          fill: true,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false }
        },
        scales: {
          x: { grid: { display: false } },
          y: { beginAtZero: true }
        }
      }
    };

    new Chart(this.revenueTrendCanvas.nativeElement, cfg);
  }

  private initSalesByMake() {
    const cfg: ChartConfiguration<'bar'> = {
      type: 'bar',
      data: {
        labels: ['BMW', 'Mercedes', 'Audi', 'Tesla', 'Porsche', 'Lexus'],
        datasets: [{
          label: 'Units Sold',
          data: [12, 10, 8, 9, 6, 3],
          borderRadius: 10
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } },
        scales: {
          x: { grid: { display: false } },
          y: { beginAtZero: true, ticks: { stepSize: 3 } }
        }
      }
    };

    new Chart(this.salesByMakeCanvas.nativeElement, cfg);
  }

  private initSalesByType() {
    const cfg: ChartConfiguration<'pie'> = {
      type: 'pie',
      data: {
        labels: ['Sedan', 'SUV', 'Coupe', 'Convertible'],
        datasets: [{
          data: [46, 29, 17, 8],
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'right' }
        }
      }
    };

    new Chart(this.salesByTypeCanvas.nativeElement, cfg);
  }

  // Funnel bars (kao na slici) – može bez chart.js
  readonly funnel = [
    { label: 'Website Visits', value: 1240, pct: 100 },
    { label: 'Inquiries Submitted', value: 208, pct: 17 },
    { label: 'Test Drives Booked', value: 86, pct: 7 },
    { label: 'Quotes Requested', value: 64, pct: 5 },
    { label: 'Sales Closed', value: 48, pct: 4 },
  ];

  readonly topVehicles = [
    { rank: 1, vehicle: '2024 Mercedes S-Class', units: 6, revenue: '$713,400', trend: '+15%' },
    { rank: 2, vehicle: '2024 BMW M3', units: 5, revenue: '$379,950', trend: '+12%' },
    { rank: 3, vehicle: '2024 Porsche 911', units: 4, revenue: '$519,600', trend: '+8%' },
    { rank: 4, vehicle: '2023 Audi RS7', units: 4, revenue: '$503,600', trend: '+5%' },
  ];
}
