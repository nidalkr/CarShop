import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { asapScheduler } from 'rxjs';
import { distinctUntilChanged, observeOn, startWith } from 'rxjs/operators';
import { LoadingBarService } from '../../../../core/services/loading-bar.service';

@Component({
  selector: 'app-fit-loading-bar',
  standalone: false,
  templateUrl: './fit-loading-bar.component.html',
  styleUrl: './fit-loading-bar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FitLoadingBarComponent {
  private readonly loadingBar = inject(LoadingBarService);

  // ✅ defer emissions -> nema ExpressionChangedAfterItHasBeenCheckedError
  readonly isLoading$ = this.loadingBar.loading$.pipe(
    startWith(false),
    distinctUntilChanged(),
    observeOn(asapScheduler)
  );
}
