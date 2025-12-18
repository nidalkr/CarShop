import { Component, inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { Router } from '@angular/router';
import { LogoutComponent } from '../../auth/logout/logout.component';
import { MatDialog } from '@angular/material/dialog';
import { take } from 'rxjs';


@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent {
  private translate = inject(TranslateService);
  auth = inject(AuthFacadeService);



  languages = [
    { code: 'bs', name: 'Bosanski', flag: '🇧🇦' },
    { code: 'en', name: 'English', flag: '🇬🇧' }
  ];



  constructor(private router: Router) {}
    onBack() {
      this.router.navigate(['/']);
    }

    private dialog = inject(MatDialog);
    openLogoutPopup() {
    const ref = this.dialog.open(LogoutComponent, {
      hasBackdrop: true,      
      autoFocus: false,
      restoreFocus: true,
      disableClose: true, 
      panelClass: 'logout-dialog-panel',
      backdropClass: 'logout-dialog-backdrop',
    });

    
    ref.componentInstance.redirectToLogin = true; 
    ref.componentInstance.compact = true;        

    
    ref.componentInstance.completed.pipe(take(1)).subscribe(() => {
      ref.close();
    });
  }

}
