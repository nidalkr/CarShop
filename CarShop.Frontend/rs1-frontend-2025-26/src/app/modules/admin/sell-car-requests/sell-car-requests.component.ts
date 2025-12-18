import { Component } from '@angular/core';

type SellCarStatus = 'pending' | 'approved' | 'rejected';

interface SellCarRequest {
  id: number;
  ownerName: string;
  submittedAt: string;

  vehicle: string;      // "2018 Toyota Camry"
  year: number;
  mileage: string;      // "50,000"
  condition: string;    // "Good"
  askingPrice: string;  // "$15,000"
  description: string;

  email: string;
  phone: string;

  status: SellCarStatus;
}

@Component({
  selector: 'app-admin-sell-car-requests',
  standalone: false,
  templateUrl: './sell-car-requests.component.html',
  styleUrls: ['./sell-car-requests.component.scss'],
})
export class AdminSellCarRequestsComponent {
  selectedStatus: 'all' | SellCarStatus = 'all';

  // TODO: kasnije zamijeni API pozivem
  sellCarRequests: SellCarRequest[] = [
    {
      id: 1,
      ownerName: 'Laura White',
      submittedAt: '2024-12-05 10:00',
      vehicle: '2018 Toyota Camry',
      year: 2018,
      mileage: '50,000',
      condition: 'Good',
      askingPrice: '$15,000',
      description: 'Clean title, no accidents, well-maintained.',
      email: 'laura@example.com',
      phone: '(555) 345-6789',
      status: 'pending',
    },
    {
      id: 2,
      ownerName: 'James Brown',
      submittedAt: '2024-12-04 12:00',
      vehicle: '2020 Audi A4',
      year: 2020,
      mileage: '72,000',
      condition: 'Very good',
      askingPrice: '$22,500',
      description: 'Full service history.',
      email: 'james@example.com',
      phone: '(555) 987-6543',
      status: 'approved',
    },
  ];

  get filteredSellCarRequests(): SellCarRequest[] {
    if (this.selectedStatus === 'all') return this.sellCarRequests;
    return this.sellCarRequests.filter(x => x.status === this.selectedStatus);
  }

  approve(req: SellCarRequest) {
    req.status = 'approved';
  }

  reject(req: SellCarRequest) {
    req.status = 'rejected';
  }
}
