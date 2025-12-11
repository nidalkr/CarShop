// src/app/layouts/public-layout/public-layout.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent {
  currentYear: string = '2025';

  isLoginOpen = false;

  openLoginModal() {
    this.isLoginOpen = true;
  }

  closeLoginModal() {
    this.isLoginOpen = false;
  }
}
