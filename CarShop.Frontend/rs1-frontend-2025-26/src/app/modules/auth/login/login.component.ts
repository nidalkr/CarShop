import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { LoginCommand } from '../../../api-services/auth/auth-api.model';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private currentUser = inject(CurrentUserService);

  hidePassword = true;

  // 🔹 ovdje čuvamo returnUrl
  private returnUrl: string | null = null;

  form = this.fb.group({
    email: ['admin@carshop.local', [Validators.required, Validators.email]],
    password: ['Admin123!', [Validators.required]],
    rememberMe: [false],
  });

  ngOnInit(): void {
    // ako je user već logiran i otvori /auth/login, možemo ga odmah baciti na default
    // if (this.currentUser.isAuthenticated()) {
    //   this.router.navigate([this.currentUser.getDefaultRoute()]);
    //   return;
    // }

    // pokupi returnUrl iz query parametara (npr. /admin/vehicles?page=2)
    this.returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');

    // ako nema returnUrl, fallback je default ruta
    // if (!this.returnUrl) {
    //   this.returnUrl = this.currentUser.getDefaultRoute();
    // }
  }

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const payload: LoginCommand = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      fingerprint: null,
    };

    this.auth.login(payload).subscribe({
      next: () => {
  this.stopLoading();

  const target = this.returnUrl || this.currentUser.getDefaultRoute();
  this.router.navigate([target]); // ili navigateByUrl, oba su ok ako target počinje sa "/"
},
      error: (err) => {
        this.stopLoading('Invalid credentials. Please try again.');
        console.error('Login error:', err);
      },
    });
  }
}
