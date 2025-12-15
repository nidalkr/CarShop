// src/app/modules/public/public-layout/public-layout.component.ts
import { Component,HostListener,AfterViewInit, ElementRef, ViewChild  } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';


@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent {
  currentYear: string = '2025';
  navHidden = false;
@ViewChild('heroVideo') heroVideo!: ElementRef<HTMLVideoElement>;
  private lastY = 0;
  private readonly delta =10;       // koliko px treba da “pomjeri” da reaguje
  private readonly revealTop = 72;  // na vrhu uvijek prikazi
  // LOGIN POPUP
  isLoginOpen = false;
  isLoginClosing = false;

  // REGISTER POPUP
  isRegisterOpen = false;
  isRegisterClosing = false;

  constructor(
    private router: Router,
    private currentUser: CurrentUserService
  ) {}

  // helper za template – da li je user logovan
  get isAuthenticated(): boolean {
    return this.currentUser.isAuthenticated();
  }

  ngAfterViewInit(): void {
  const v = this.heroVideo?.nativeElement;
  if (!v) return;

  // HARD mute (radi i kad browser “ignoriše” samo atribut)
  v.muted = true;
  v.defaultMuted = true;
  v.volume = 0;

  // u nekim slučajevima autoplay ne krene bez eksplicitnog play()
  v.play().catch(() => {});
}

  // =========================
  // LOGIN MODAL
  // =========================
  openLoginModal(): void {
    this.isLoginClosing = false;
    this.isLoginOpen = true;
  }

  closeLoginModal(): void {
    if (this.isLoginClosing) return;
    this.isLoginClosing = true;

    setTimeout(() => {
      this.isLoginOpen = false;
      this.isLoginClosing = false;
    }, 220);
  }

  // =========================
  // REGISTER MODAL
  // =========================
  openRegisterModal(): void {
    this.isRegisterClosing = false;
    this.isRegisterOpen = true;
  }

  closeRegisterModal(): void {
    if (this.isRegisterClosing) return;
    this.isRegisterClosing = true;

    setTimeout(() => {
      this.isRegisterOpen = false;
      this.isRegisterClosing = false;
    }, 220);
  }

  // poziva se iz <app-login> (createAccount)
  handleCreateAccountFromLogin(): void {
    // zatvori login popup
    this.isLoginOpen = false;
    this.isLoginClosing = false;

    // otvori register wizard popup
    this.openRegisterModal();
  }

  // poziva se iz <app-register> (signIn)
  handleSignInFromRegister(): void {
    this.isRegisterOpen = false;
    this.isRegisterClosing = false;

    this.openLoginModal();
  }

  // =========================
  // LOGOUT
  // =========================
  logout(): void {
    // postojeći LogoutComponent na /auth/logout ruti
    this.router.navigate(['/auth/logout']);
  }

  // =========================
  // NAVBAR HIDING ON SCROLL
  // =========================
  @HostListener('window:scroll', [])
  onScroll() {
    const y = window.scrollY || document.documentElement.scrollTop || 0;

    // uvijek prikazi na samom vrhu
    if (y <= this.revealTop) {
      this.navHidden = false;
      this.lastY = y;
      return;
    }

    // stabilizacija (da ne treperi)
    if (Math.abs(y - this.lastY) < this.delta) return;

    if (y > this.lastY) {
      // scroll down -> sakrij
      this.navHidden = true;
    } else {
      // scroll up -> prikazi
      this.navHidden = false;
    }

    this.lastY = y;
  }
}
