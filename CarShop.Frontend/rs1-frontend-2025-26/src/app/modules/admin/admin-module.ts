import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AdminRoutingModule } from './admin-routing-module';
import { SharedModule } from '../shared/shared-module';

import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { AdminSettingsComponent } from './admin-settings/admin-settings.component';


import { AdminDashboardComponent } from './dashboard/admin-dashboard/admin-dashboard.component';
import { AdminTestDrivesComponent } from './test-drives/admin-test-drives/admin-test-drives.component';
import { AdminQuoteRequestsComponent } from './quote-requests/admin-quote-requests/admin-quote-requests.component';
import { AdminSellCarRequestsComponent} from './sell-car-requests/sell-car-requests.component';
import { AdminSalesStatsComponent } from './sales-stats/sales-stats.component';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    AdminSettingsComponent,
    AdminDashboardComponent,
    AdminTestDrivesComponent,
    AdminQuoteRequestsComponent,
    AdminSellCarRequestsComponent,
    AdminSalesStatsComponent
  ],
  imports: [
    CommonModule,
    RouterModule,   
    FormsModule, 
    AdminRoutingModule,
    SharedModule,   
    
  ]
})
export class AdminModule {}
