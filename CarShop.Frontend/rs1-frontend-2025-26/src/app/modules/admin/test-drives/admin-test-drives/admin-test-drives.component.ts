import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-test-drives',
  standalone: false,
  
  templateUrl: './admin-test-drives.component.html',
  styleUrls: ['./admin-test-drives.component.scss']
})
export class AdminTestDrivesComponent {
  selectedStatus: 'all' | 'pending' | 'approved' | 'completed' = 'all';

  testDrives = [
    {
      id: 1,
      customerName: 'John Doe',
      submittedAt: '2024-12-05 09:30',
      vehicle: '2023 BMW 3 Series',
      location: 'Main Showroom',
      dateTime: '2024-12-15 10:00 AM',
      contact: '(555) 123-4567',
      status: 'pending'
    },
    {
      id: 2,
      customerName: 'Sarah Johnson',
      submittedAt: '2024-12-04 14:20',
      vehicle: '2024 Tesla Model 3',
      location: 'Downtown Location',
      dateTime: '2024-12-16 02:00 PM',
      contact: '(555) 987-6543',
      status: 'approved'
    }
  ];

  get filteredTestDrives() {
    if (this.selectedStatus === 'all') return this.testDrives;
    return this.testDrives.filter(td => td.status === this.selectedStatus);
  }

  approve(td: any) {
    td.status = 'approved';
  }

  reject(td: any) {
    td.status = 'rejected';
  }

  complete(td: any) {
    td.status = 'completed';
  }
}
