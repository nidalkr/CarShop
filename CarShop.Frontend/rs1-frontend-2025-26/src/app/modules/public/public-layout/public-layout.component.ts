// src/app/modules/public/public-layout/public-layout.component.ts
import { Component, AfterViewInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent implements AfterViewInit, OnDestroy {
  currentYear: string = '2025';
  navHidden = false;

  private lastY = 0;
  private readonly delta = 10;
  private readonly revealTop = 10;

  private ticking = false;
  private latestY = 0;

  private removeFns: Array<() => void> = [];

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

  get isAuthenticated(): boolean {
    return this.currentUser.isAuthenticated();
  }

  ngAfterViewInit(): void {
    // safety: da se ne zalijepi lock
    document.body.classList.remove('is-scroll-locked');

    this.lastY = this.getAnyScrollTop();
    this.attachScrollListeners();
  }

  ngOnDestroy(): void {
    this.removeFns.forEach((fn) => fn());
    this.removeFns = [];
  }

  // =========================
  // LOGIN MODAL
  // =========================
  openLoginModal(): void {
    this.isLoginClosing = false;
    this.isLoginOpen = true;
    this.setScrollLock(true);
  }

  closeLoginModal(): void {
    if (this.isLoginClosing) return;
    this.isLoginClosing = true;

    setTimeout(() => {
      this.isLoginOpen = false;
      this.isLoginClosing = false;
      this.setScrollLock(false);
    }, 220);
  }

  // =========================
  // REGISTER MODAL
  // =========================
  openRegisterModal(): void {
    this.isRegisterClosing = false;
    this.isRegisterOpen = true;
    this.setScrollLock(true);
  }

  closeRegisterModal(): void {
    if (this.isRegisterClosing) return;
    this.isRegisterClosing = true;

    setTimeout(() => {
      this.isRegisterOpen = false;
      this.isRegisterClosing = false;
      this.setScrollLock(false);
    }, 220);
  }

  handleCreateAccountFromLogin(): void {
    this.isLoginOpen = false;
    this.isLoginClosing = false;
    this.openRegisterModal();
  }

  handleSignInFromRegister(): void {
    this.isRegisterOpen = false;
    this.isRegisterClosing = false;
    this.openLoginModal();
  }

  logout(): void {
    this.router.navigate(['/auth/logout']);
  }

  // =========================
  // NAVBAR HIDING ON SCROLL
  // =========================

  private setScrollLock(lock: boolean): void {
    document.body.classList.toggle('is-scroll-locked', lock);
  }

  private getAnyScrollTop(): number {
    const win = window.scrollY || 0;
    const doc = document.documentElement?.scrollTop || 0;
    const body = document.body?.scrollTop || 0;
    return Math.max(win, doc, body);
  }

  private attachScrollListeners(): void {
    const handler = () => this.schedule(this.getAnyScrollTop());
    const opts: AddEventListenerOptions = { passive: true, capture: true };

    const add = (target: any) => {
      target.addEventListener('scroll', handler, opts);
      this.removeFns.push(() => target.removeEventListener('scroll', handler, opts));
    };

    // hvata scroll i kad je na window, i kad je na body/html, i kad je na nekom wrapperu
    add(window);
    add(document);
    add(document.documentElement);
    add(document.body);
  }

  private schedule(y: number): void {
    this.latestY = Math.max(0, y);

    if (this.ticking) return;
    this.ticking = true;

    requestAnimationFrame(() => {
      this.ticking = false;

      if (this.isLoginOpen || this.isRegisterOpen) return;

      this.handleScroll(this.latestY);
    });
  }

  private handleScroll(y: number): void {
    if (y <= this.revealTop) {
      this.navHidden = false;
      this.lastY = y;
      return;
    }

    if (Math.abs(y - this.lastY) < this.delta) return;

    this.navHidden = y > this.lastY; // down hide, up show
    this.lastY = y;
  }
}
