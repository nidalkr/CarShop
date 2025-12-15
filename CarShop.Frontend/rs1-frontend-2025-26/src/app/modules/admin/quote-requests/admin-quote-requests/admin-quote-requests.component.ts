import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-admin-quote-requests',
  standalone: false,
  
  templateUrl: './admin-quote-requests.component.html',
  styleUrls: ['./admin-quote-requests.component.scss']
})
export class AdminQuoteRequestsComponent implements OnInit {

  // =========================
  // FILTER STATE
  // =========================
  activeStatus: 'all' | 'pending' | 'responded' = 'all';
  searchTerm = '';

  // =========================
  // DATA MODEL (mock / replace with API)
  // =========================
  quoteRequests: QuoteRequest[] = [
    {
      id: 1,
      fullName: 'Emily Rodriguez',
      status: 'pending',
      submittedAt: new Date('2024-12-05T11:15:00'),
      vehicle: '2024 Audi A4',
      inquiryType: 'Purchase',
      message: 'Interested in financing options and trade-in value for my 2019 Honda Accord.',
      email: 'emily@example.com',
      phone: '(555) 234-5678'
    },
    {
      id: 2,
      fullName: 'David Miller',
      status: 'responded',
      submittedAt: new Date('2024-12-04T13:40:00'),
      vehicle: '2023 Lexus ES',
      inquiryType: 'Lease',
      message: 'Looking for lease terms and monthly payment options.',
      email: 'david@example.com',
      phone: '(555) 987-6543'
    }
  ];

  filteredRequests: QuoteRequest[] = [];

  // =========================
  // LIFECYCLE
  // =========================
  ngOnInit(): void {
    this.applyFilters();
  }

  // =========================
  // FILTER LOGIC
  // =========================
  setStatus(status: 'all' | 'pending' | 'responded'): void {
    this.activeStatus = status;
    this.applyFilters();
  }

  applyFilters(): void {
    const term = this.searchTerm.trim().toLowerCase();

    this.filteredRequests = this.quoteRequests.filter(q => {
      const statusMatch =
        this.activeStatus === 'all'
          ? true
          : q.status === this.activeStatus;

      const searchMatch = term
        ? `${q.fullName} ${q.email} ${q.vehicle} ${q.inquiryType} ${q.phone}`
            .toLowerCase()
            .includes(term)
        : true;

      return statusMatch && searchMatch;
    });
  }

  // =========================
  // ACTIONS
  // =========================
  markAsResponded(item: QuoteRequest): void {
    item.status = 'responded';

    // TODO: API call
    // this.quoteService.markAsResponded(item.id).subscribe(...)

    this.applyFilters();
  }

  close(item: QuoteRequest): void {
    // TODO: backend action (archive / delete)
    this.quoteRequests = this.quoteRequests.filter(q => q.id !== item.id);
    this.applyFilters();
  }

}

/* =========================
   INTERFACE
   ========================= */
export interface QuoteRequest {
  id: number;
  fullName: string;
  status: 'pending' | 'responded';
  submittedAt: Date;
  vehicle: string;
  inquiryType: string;
  message: string;
  email: string;
  phone: string;
}
