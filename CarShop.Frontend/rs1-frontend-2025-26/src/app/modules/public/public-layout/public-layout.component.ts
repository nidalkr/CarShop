// src/app/modules/public/public-layout/public-layout.component.ts
import {
  Component,
  AfterViewInit,
  ElementRef,
  ViewChild,
  OnDestroy,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  NgZone,
} from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicLayoutComponent implements AfterViewInit, OnDestroy {
  currentYear: string = '2025';
  navHidden = false;

  @ViewChild('heroVideo') heroVideo!: ElementRef<HTMLVideoElement>;

  // HOW IT WORKS
  @ViewChild('howWorksSection') howWorksSection!: ElementRef<HTMLElement>;
  howWorksVisible = false;

  // PREMIUM EXPERIENCE
  @ViewChild('premiumSection') premiumSection!: ElementRef<HTMLElement>;
  premiumVisible = false;

  // VISIT
  @ViewChild('visitSection') visitSection!: ElementRef<HTMLElement>;
  visitVisible = false;

  // SELL
  @ViewChild('sellSection') sellSection!: ElementRef<HTMLElement>;
  sellVisible = false;
  @ViewChild('sellCard') sellCard!: ElementRef<HTMLElement>;

  @ViewChild('collectionsSection') collectionsSection!: ElementRef<HTMLElement>;
  collectionsVisible = false;

  // MAP lazy mount (najveći “spike” zna biti ovdje)
  visitMapLoaded = false;

  private revealObserver?: IntersectionObserver;
  private collectionsObserver?: IntersectionObserver;
  // Navbar scroll perf
  private lastY = 0;
  private ticking = false;
  private readonly delta = 10;
  private readonly revealTop = 72;

  // LOGIN POPUP
  isLoginOpen = false;
  isLoginClosing = false;

  // REGISTER POPUP
  isRegisterOpen = false;
  isRegisterClosing = false;

  // LOGOUT POPUP
  isLogoutOpen = false;
  isLogoutClosing = false;

  constructor(
    private currentUser: CurrentUserService,
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef
  ) {}

  get isAdmin(): boolean {
    return this.currentUser.isAdmin();
  }

  get isAuthenticated(): boolean {
    return this.currentUser.isAuthenticated();
  }

  // ===== DATA =====
  collectionCards = [
    { count: '120+ vehicles', title: 'Luxury Sedans', desc: 'Premium comfort meets performance', image: 'assets/luxury.jpg', link: '/inventory?category=sedan' },
    { count: '85+ vehicles', title: 'Sports Cars', desc: 'Adrenaline-pumping performance', image: 'assets/sports.jpg', link: '/inventory?category=sports' },
    { count: '60+ vehicles', title: 'Electric Vehicles', desc: 'Sustainable luxury driving', image: 'assets/ev.jpg', link: '/inventory?category=ev' },
    { count: '95+ vehicles', title: 'SUV & Trucks', desc: 'Power and versatility combined', image: 'assets/suv.jpg', link: '/inventory?category=suv' },
  ];

  howSteps = [
    { no: '01', icon: 'search', title: 'Browse Collection', desc: 'Explore our premium inventory online or visit our showroom' },
    { no: '02', icon: 'calendar_month', title: 'Schedule Test Drive', desc: 'Book your test drive and experience the vehicle firsthand' },
    { no: '03', icon: 'task_alt', title: 'Get Financing', desc: 'Work with our team to secure the best financing options' },
    { no: '04', icon: 'directions_car', title: 'Drive Home', desc: 'Complete paperwork and drive away in your dream car' },
  ];

  premiumFeatures = [
    { icon: 'verified_user', title: 'Certified Quality', desc: 'Every vehicle undergoes rigorous 200-point inspection', iconBg: 'linear-gradient(135deg, #00b4dd 0%, #0052af 100%)', iconGlow: 'rgba(0, 160, 255, 0.22)' },
    { icon: 'workspace_premium', title: 'Best Prices', desc: 'Competitive pricing with transparent financing options', iconBg: 'linear-gradient(135deg, #8b5cf6 0%, #ff4fd8 100%)', iconGlow: 'rgba(255, 79, 216, 0.18)' },
    { icon: 'bolt', title: 'Fast Delivery', desc: 'Quick processing and home delivery within 48 hours', iconBg: 'linear-gradient(135deg, #ff7a00 0%, #ff2d00 100%)', iconGlow: 'rgba(255, 122, 0, 0.18)' },
    { icon: 'support_agent', title: 'Expert Support', desc: '24/7 dedicated team ready to assist you anytime', iconBg: 'linear-gradient(135deg, #00c853 0%, #00bfa5 100%)', iconGlow: 'rgba(0, 200, 120, 0.18)' },
  ];

  scrollTo(id: string): void {
    const el = document.getElementById(id);
    if (!el) return;

    const headerOffset = 88;
    const y = el.getBoundingClientRect().top + window.pageYOffset - headerOffset;
    window.scrollTo({ top: y, behavior: 'smooth' });
  }

  ngAfterViewInit(): void {
    // VIDEO hard mute + autoplay kick
    const v = this.heroVideo?.nativeElement;
    if (v) {
      v.muted = true;
      v.defaultMuted = true;
      v.volume = 0;
      v.play().catch(() => {});
    }

    this.setupNavbarScroll();
    this.setupRevealObserver();
  }

  // ========== PERF: navbar scroll outside Angular ==========
  private setupNavbarScroll(): void {
    this.lastY = window.scrollY || document.documentElement.scrollTop || 0;

    this.ngZone.runOutsideAngular(() => {
      window.addEventListener('scroll', this.onWindowScroll, { passive: true });
    });
  }

  private onWindowScroll = (): void => {
    if (this.ticking) return;
    this.ticking = true;

    requestAnimationFrame(() => {
      this.ticking = false;

      const y = window.scrollY || document.documentElement.scrollTop || 0;

      let nextHidden = this.navHidden;

      if (y <= this.revealTop) {
        nextHidden = false;
      } else if (Math.abs(y - this.lastY) >= this.delta) {
        nextHidden = y > this.lastY; // down -> hide, up -> show
      }

      this.lastY = y;

      if (nextHidden !== this.navHidden) {
        this.ngZone.run(() => {
          this.navHidden = nextHidden;
          this.cdr.markForCheck();
        });
      }
    });
  };

  // ========== Reveal: jedan observer za sve, bez dupliranja ==========
  // ========== Reveal: jedan observer za sve, bez dupliranja ==========
private setupRevealObserver(): void {
  const howEl = this.howWorksSection?.nativeElement;
  const premEl = this.premiumSection?.nativeElement;
  const visitEl = this.visitSection?.nativeElement;
  const sellEl = this.sellCard?.nativeElement ?? this.sellSection?.nativeElement;

  // ✅ NEW: Collections
  const collectionsEl = this.collectionsSection?.nativeElement;

  // fallback: ako nema IO
  if (!('IntersectionObserver' in window)) {
    this.howWorksVisible = true;
    this.premiumVisible = true;
    this.visitVisible = true;
    this.sellVisible = true;

    // ✅ NEW
    this.collectionsVisible = true;

    this.visitMapLoaded = true;
    this.cdr.markForCheck();
    return;
  }

  this.ngZone.runOutsideAngular(() => {
    this.revealObserver = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (!entry.isIntersecting) continue;

          const target = entry.target as HTMLElement;

          this.ngZone.run(() => {
            if (target === collectionsEl) this.collectionsVisible = true; // ✅ NEW
            if (target === howEl) this.howWorksVisible = true;
            if (target === premEl) this.premiumVisible = true;

            if (target === visitEl) {
              this.visitVisible = true;
              this.queueMapLoad();
            }

            if (target === sellEl) this.sellVisible = true;

            this.cdr.markForCheck();
          });

          this.revealObserver?.unobserve(target);
        }
      },
      { threshold: 0.14, rootMargin: '0px 0px -6% 0px' }
    );

    // ✅ NEW: observe collections
    if (collectionsEl) this.revealObserver.observe(collectionsEl);

    if (howEl) this.revealObserver.observe(howEl);
    if (premEl) this.revealObserver.observe(premEl);
    if (visitEl) this.revealObserver.observe(visitEl);
    if (sellEl) this.revealObserver.observe(sellEl);
  });
}


  private queueMapLoad(): void {
    if (this.visitMapLoaded) return;

    const load = () => {
      this.visitMapLoaded = true;
      this.cdr.markForCheck();
    };

    // učitaj mapu u idle (da ne “zapne” baš u scroll trenutku)
    this.ngZone.runOutsideAngular(() => {
      const w = window as any;
      if (typeof w.requestIdleCallback === 'function') {
        w.requestIdleCallback(() => this.ngZone.run(load), { timeout: 1200 });
      } else {
        setTimeout(() => this.ngZone.run(load), 250);
      }
    });
  }

  // opcionalno: ako želiš “Load map” klik
  loadMapNow(): void {
    if (this.visitMapLoaded) return;
    this.visitMapLoaded = true;
    this.cdr.markForCheck();
  }

  ngOnDestroy(): void {
    window.removeEventListener('scroll', this.onWindowScroll);
    this.revealObserver?.disconnect();
    this.collectionsObserver?.disconnect();
  }

  // ========== MODALI ==========
  openLoginModal(): void {
    this.isLoginClosing = false;
    this.isLoginOpen = true;
    this.cdr.markForCheck();
  }

  closeLoginModal(): void {
    if (this.isLoginClosing) return;
    this.isLoginClosing = true;
    this.cdr.markForCheck();

    setTimeout(() => {
      this.isLoginOpen = false;
      this.isLoginClosing = false;
      this.cdr.markForCheck();
    }, 220);
  }

  openRegisterModal(): void {
    this.isRegisterClosing = false;
    this.isRegisterOpen = true;
    this.cdr.markForCheck();
  }

  closeRegisterModal(): void {
    if (this.isRegisterClosing) return;
    this.isRegisterClosing = true;
    this.cdr.markForCheck();

    setTimeout(() => {
      this.isRegisterOpen = false;
      this.isRegisterClosing = false;
      this.cdr.markForCheck();
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

  openLogoutModal(): void {
    this.isLogoutClosing = false;
    this.isLogoutOpen = true;
    this.cdr.markForCheck();
  }

  closeLogoutModal(): void {
    if (this.isLogoutClosing) return;
    this.isLogoutClosing = true;
    this.cdr.markForCheck();

    setTimeout(() => {
      this.isLogoutOpen = false;
      this.isLogoutClosing = false;
      this.cdr.markForCheck();
    }, 220);
  }

  handleLogoutCompleted(): void {
    this.closeLogoutModal();
  }

  logout(): void {
    this.openLogoutModal();
  }
}
