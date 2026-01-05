import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from '../../../../environments/environment';
import { finalize, map } from 'rxjs/operators';
import { BehaviorSubject, combineLatest, forkJoin, Subscription } from 'rxjs';
// paginator uses plain vm object

import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { BasePagedQuery } from '../../../core/models/paging/base-paged-query';

import { FitConfirmDialogComponent } from '../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import {
  DialogButton,
  DialogConfig,
  DialogType,
  DialogResult
} from '../../shared/models/dialog-config.model';

import { CarsApiService } from '../../../api-services/cars/cars-api.service';
import { CarDetailsDto, CreateCarRequest, UpdateCarRequest } from '../../../api-services/cars/cars-api.model';
import { BrandsApiService } from '../../../api-services/brands/brands-api.service';
import { CategoriesApiService } from '../../../api-services/categories/categories-api.service';
import { StatusesApiService } from '../../../api-services/statuses/statuses-api.service';
import { LookupItemDto as BrandDto } from '../../../api-services/brands/brands-api.model';
import { LookupItemDto as CategoryDto } from '../../../api-services/categories/categories-api.model';
import { StatusLookupDto } from '../../../api-services/statuses/statuses-api.model';

type VehicleStatus = 'In Stock' | 'Limited' | 'Sold';
type VehicleCondition = 'New' | 'Used';


interface Vehicle {
  id: number;
  make: string;
  model: string;
  year: number;
  condition: VehicleCondition;
  fuelType: string;
  transmission: string;
  hp: number;
  miles: number;
  price: number;
  status: VehicleStatus;
  categoryName?: string;

  imageUrl: string;
  images: string[];

  engine?: string;
  drivetrain?: string;
  mpg?: string;
  seating?: number;
  doors?: number;

  stockNumber?: string;
  inventoryLocation?: string;
  quantityInStock?: number;
  msrp?: number | null;

  exteriorColor?: string;
  interiorColor?: string;
  description?: string;
  features?: string[];

  _raw?: CarDetailsDto;
}

interface NewVehicleForm {
  brandId: number;
  categoryId: number;
  carStatusId: number;

  vin: string;
  stockNumber: string;
  inventoryLocation: string;
  doors: number;
  quantityInStock: number;

  make: string;
  model: string;
  year: number;
  price: number;

  engine: string;
  hp: number;
  transmission: string;
  drivetrain: string;
  fuelType: string;
  mpg: string;
  miles: number;
  condition: VehicleCondition;
  seating: number;
  exteriorColor: string;
  interiorColor: string;

  features: string;
  description: string;
  status: VehicleStatus;
  images: File[];
}

class InventoryPaginatorAdapter extends BaseListPagedComponent<Vehicle, BasePagedQuery> {
  constructor(private cmp: AdminInventoryManagementComponent) {
    super();
    this.request = new BasePagedQuery();
    this.request.paging.page = cmp.page;
    this.request.paging.pageSize = cmp.pageSize;
    this.totalItems = cmp.totalCount;
    this.totalPages = cmp.totalPages;
  }
  protected loadPagedData(): void {
    this.cmp.page = this.request.paging.page;
    this.cmp.pageSize = this.request.paging.pageSize;
    this.cmp.loadCars();
  }
}

@Component({
  selector: 'app-admin-inventory-management',
  standalone: false,
  templateUrl: './admin-inventory-management.component.html',
  styleUrls: ['./admin-inventory-management.component.scss'],
})
export class AdminInventoryManagementComponent implements OnInit, OnDestroy {
  constructor(
    private dialog: MatDialog,
    private carsApi: CarsApiService,
    private brandsApi: BrandsApiService,
    private categoriesApi: CategoriesApiService,
    private statusesApi: StatusesApiService
  ) {}

  isLoading = false;
  isViewLoading = false;
  loadError: string | null = null;
  viewError: string | null = null;

  query = '';
  selectedMake: string = 'all';
  selectedFuelType: string = 'all';
  selectedCondition: string = 'all';

  makeOptions: string[] = [];
  fuelTypeOptions: string[] = [];
  conditionOptions: VehicleCondition[] = ['New', 'Used'];

  vehicles: Vehicle[] = [];
  filteredVehicles: Vehicle[] = [];
  private vehicles$ = new BehaviorSubject<Vehicle[]>([]);
  private filters$ = new BehaviorSubject<{ q: string; make: string; fuel: string; cond: string }>({
    q: '',
    make: 'all',
    fuel: 'all',
    cond: 'all'
  });
  private filtersSub?: Subscription;

  page = 1;

  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  paginatorVm: InventoryPaginatorAdapter = new InventoryPaginatorAdapter(this);

  showViewModal = false;
  selectedVehicle: Vehicle | null = null;
  activeImageIndex = 0;
  zoomedImage: string | null = null;
  zoomScale = 1.8;
  zoomOrigin = 'center center';
  lensVisible = false;
  lensX = 0;
  lensY = 0;
  lensSize = 140;
  lensZoom = 2;
  lensBgPos = '0% 0%';
  lensBgImage = '';

  private onKeyDown = (e: KeyboardEvent) => {
    if (e.key === 'Escape' && this.showViewModal) this.closeViewModal();
  };

  showAddWizard = false;
  wizardStep = 1;

  isEditMode = false;
  editingVehicleId: number | null = null;
  editingPrimaryImageUrl: string | null = null;
  editingGalleryUrls: string[] = [];

  formErrors: string[] = [];
  primaryUploadIndex = 0;

  brandOptions: BrandDto[] = [];
  categoryOptions: CategoryDto[] = [];
  carStatusOptions: StatusLookupDto[] = [];

  newVehicle: NewVehicleForm = this.createEmptyForm();

  ngOnInit(): void {
    this.loadLookups();
    this.loadCars();
    window.addEventListener('keydown', this.onKeyDown);
    this.filtersSub = combineLatest([this.vehicles$, this.filters$])
  .pipe(
    map(([items, f]) => {
      const q = f.q.trim().toLowerCase();
      return items.filter(v => {
        const matchesQuery = !q || `${v.make} ${v.model} ${v.year}`.toLowerCase().includes(q);
        const matchesMake = f.make === 'all' || v.make === f.make;
        const matchesFuel = f.fuel === 'all' || v.fuelType === f.fuel;
        const matchesCond = f.cond === 'all' || v.condition === (f.cond as VehicleCondition);
        return matchesQuery && matchesMake && matchesFuel && matchesCond;
      });
    })
  )
  .subscribe(list => (this.filteredVehicles = list));
  }

  ngOnDestroy(): void {
    window.removeEventListener('keydown', this.onKeyDown);
    document.body.classList.remove('inv-modal-open');
    this.filtersSub?.unsubscribe();
  }

  private loadLookups(): void {
    forkJoin({
      brands: this.brandsApi.list(),
      categories: this.categoriesApi.list(),
      statuses: this.statusesApi.list()
    }).subscribe({
      next: res => {
        this.brandOptions = res.brands;
        this.categoryOptions = res.categories;
        this.carStatusOptions = res.statuses;

        if (!this.newVehicle.brandId && this.brandOptions.length) {
          this.newVehicle.brandId = this.brandOptions[0].id;
        }
        if (!this.newVehicle.categoryId && this.categoryOptions.length) {
          this.newVehicle.categoryId = this.categoryOptions[0].id;
        }
        if (!this.newVehicle.carStatusId && this.carStatusOptions.length) {
          this.newVehicle.carStatusId = this.carStatusOptions[0].id;
        }
      },
      error: err => console.error('Lookup load failed', err)
    });
  }

  loadCars(): void {
    this.isLoading = true;
    this.loadError = null;
    this.paginatorVm.isLoading = true;

    this.carsApi.getAll(this.page, this.pageSize)
      .pipe(finalize(() => {
        this.isLoading = false;
        this.syncPaginatorVm();
      }))
      .subscribe({
        next: (res: any) => {
          const items = Array.isArray(res?.items) ? res.items : [];
          this.totalCount = Number(res?.totalCount) || items.length;          
          this.page = Number(res?.page) || this.page;
          this.totalPages = Math.max(1, Math.ceil((this.totalCount || 0) / (this.pageSize || 1)));
          this.vehicles = items.map((x: any) => this.mapListItemToVehicle(x));
          this.vehicles$.next(this.vehicles);
          this.rebuildFilterOptions();
          this.emitFilters();

        },
        error: err => {
          console.error(err);
          this.loadError = 'Failed to load vehicles.';
          this.vehicles = [];
          this.filteredVehicles = [];
          this.vehicles$.next([]);
          this.rebuildFilterOptions();
        }
      });
  }

  applyFilters(): void {
  this.emitFilters();
}

resetFilters(): void {
  this.query = '';
  this.selectedMake = 'all';
  this.selectedFuelType = 'all';
  this.selectedCondition = 'all';
  this.emitFilters();
}


  goToPage(p: number): void {
    if (p < 1) return;
    this.page = p;
    this.loadCars();
  }

  changePageSize(size: number): void {
    if (size <= 0) return;
    this.pageSize = size;
    this.page = 1;
    this.loadCars();
  }

  private rebuildFilterOptions(): void {
    this.makeOptions = this.unique(this.vehicles.map((x) => x.make).filter(Boolean));
    this.fuelTypeOptions = this.unique(this.vehicles.map((x) => x.fuelType).filter(Boolean));
  }
  private emitFilters(): void {
  this.filters$.next({
    q: this.query,
    make: this.selectedMake,
    fuel: this.selectedFuelType,
    cond: this.selectedCondition,
  });
}


  private unique(arr: string[]): string[] {
    return Array.from(new Set(arr)).sort((a, b) => a.localeCompare(b));
  }

  viewVehicle(v: Vehicle): void {
    this.resetCarouselIndex();
    this.zoomedImage = null;
    this.showViewModal = true;
    this.isViewLoading = true;
    this.viewError = null;
    document.body.classList.add('inv-modal-open');

    this.carsApi.getById(v.id)
      .pipe(finalize(() => (this.isViewLoading = false)))
      .subscribe({
        next: dto => {
          const full = this.mapDetailsDtoToVehicle(dto);
          full.make = (dto as any).brandName ?? v.make ?? '-';
          if (!full.images?.length && full.imageUrl) {
            full.images = [full.imageUrl];
          }
          this.selectedVehicle = full;
        },
        error: err => {
          console.error(err);
          this.viewError = 'Failed to load vehicle details';
          this.selectedVehicle = !v.images?.length && v.imageUrl ? { ...v, images: [v.imageUrl] } : v;
        }
      });
  }

  closeViewModal(): void {
    this.showViewModal = false;
    this.selectedVehicle = null;
    this.viewError = null;
    this.isViewLoading = false;
    this.activeImageIndex = 0;
    this.zoomedImage = null;
    document.body.classList.remove('inv-modal-open');
  }

  setStatus(v: Vehicle, status: VehicleStatus): void {
    v.status = status;
    this.applyFilters();
  }

  badgeClass(status: VehicleStatus): string {
    return status === 'In Stock' ? 'ok' : status === 'Limited' ? 'warn' : 'bad';
  }

  confirmDeleteVehicle(v: Vehicle): void {
    const dialogRef = this.dialog.open(FitConfirmDialogComponent, {
      width: '420px',
      disableClose: true,
      data: <DialogConfig>{
        type: DialogType.WARNING,
        title: 'Delete vehicle',
        message: 'Are you sure you want to delete this vehicle?\nThis action cannot be undone.',
        buttons: [
          { type: DialogButton.CANCEL, label: 'No, cancel' },
          { type: DialogButton.DELETE, label: 'Yes, delete' }
        ]
      }
    });

    dialogRef.afterClosed().subscribe((result: DialogResult) => {
      if (!result) return;
      if (result.button === DialogButton.DELETE) {
        this.deleteVehicle(v);
      }
    });
  }

  private deleteVehicle(v: Vehicle): void {
    const api: any = this.carsApi as any;
    const hasDelete = api && typeof api.delete === 'function';

    if (!hasDelete) {
      this.vehicles = this.vehicles.filter((x) => x.id !== v.id);
      this.rebuildFilterOptions();
      this.applyFilters();

      if (this.selectedVehicle?.id === v.id) this.closeViewModal();
      if (this.editingVehicleId === v.id) this.closeWizard();
      return;
    }

    this.isLoading = true;
    api.delete(v.id)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.showSuccessDialog('Vehicle deleted', 'The vehicle has been removed from inventory.');
          this.loadCars();
          if (this.selectedVehicle?.id === v.id) this.closeViewModal();
          if (this.editingVehicleId === v.id) this.closeWizard();
        },
        error: (err: any) => {
          console.error(err);
          this.loadError = 'Failed to delete vehicle.';
          this.loadCars();
        }
      });
  }

  private showSuccessDialog(title: string, message: string): void {
    this.dialog.open(FitConfirmDialogComponent, {
      width: '420px',
      data: <DialogConfig>{
        type: DialogType.SUCCESS,
        title,
        message,
        buttons: [{ type: DialogButton.OK }]
      }
    });
  }

  addVehicle(): void {
    this.isEditMode = false;
    this.editingVehicleId = null;
    this.editingPrimaryImageUrl = null;
    this.editingGalleryUrls = [];
    this.formErrors = [];
    this.primaryUploadIndex = 0;

    this.showAddWizard = true;
    this.wizardStep = 1;
    this.newVehicle = this.createEmptyForm();

    setTimeout(() => {
      document.querySelector('.inv-wizard-card')?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 0);
  }

  editVehicle(v: Vehicle): void {
    this.isEditMode = true;
    this.editingVehicleId = v.id;
    this.editingPrimaryImageUrl = v.imageUrl || null;
    this.editingGalleryUrls = (v.images ?? []).slice(1);
    this.formErrors = [];
    this.primaryUploadIndex = 0;

    this.showAddWizard = true;
    this.wizardStep = 1;

    this.isLoading = true;
    this.carsApi.getById(v.id)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: dto => {
          this.newVehicle = this.mapDetailsDtoToForm(dto);
          const imgs = this.mapDetailsDtoToVehicle(dto).images;
          this.editingPrimaryImageUrl = imgs[0] || null;
          this.editingGalleryUrls = imgs.slice(1);
        },
        error: err => {
          console.error(err);
          this.newVehicle = {
            brandId: this.brandOptions[0]?.id ?? 0,
            categoryId: this.categoryOptions[0]?.id ?? 0,
            carStatusId: this.carStatusOptions[0]?.id ?? 0,
            vin: v.stockNumber ?? '',
            stockNumber: v.stockNumber ?? '',
            inventoryLocation: v.inventoryLocation ?? '',
            doors: v.doors ?? 4,
            quantityInStock: v.quantityInStock ?? 1,

            make: this.getBrandNameById(this.brandOptions[0]?.id ?? 0) ?? '',
            model: v.model ?? '',
            year: v.year ?? new Date().getFullYear(),
            price: v.price ?? 0,

            engine: v.engine ?? '',
            hp: v.hp ?? 0,
            transmission: v.transmission ?? 'Automatic',
            drivetrain: v.drivetrain ?? '',
            fuelType: v.fuelType ?? 'Gasoline',
            mpg: v.mpg ?? '',

            miles: v.miles ?? 0,
            condition: v.condition ?? 'Used',
            seating: v.seating ?? 5,
            exteriorColor: v.exteriorColor ?? '',
            interiorColor: v.interiorColor ?? '',

            features: (v.features ?? []).join(', '),
            description: v.description ?? '',
            status: v.status ?? 'In Stock',
            images: [],
          };
        }
      });

    setTimeout(() => {
      document.querySelector('.inv-wizard-card')?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 0);
  }

  closeWizard(): void {
    this.showAddWizard = false;
    this.wizardStep = 1;

    this.isEditMode = false;
    this.editingVehicleId = null;
    this.newVehicle = this.createEmptyForm();
  }

  nextStep(): void {
    if (this.wizardStep < 4) this.wizardStep++;
  }

  prevStep(): void {
    if (this.wizardStep > 1) this.wizardStep--;
  }

  onImagesSelected(e: Event): void {
    const input = e.target as HTMLInputElement;
    const files = input.files ? Array.from(input.files) : [];
    this.newVehicle.images = files;
    this.primaryUploadIndex = 0;
  }

  saveNewVehicle(): void {
    const api: any = this.carsApi as any;
    const hasCreate = api && typeof api.create === 'function';

    this.formErrors = this.validateForm(this.newVehicle);
    if (this.formErrors.length > 0) {
      this.showValidationDialog();
      return;
    }

    if (!hasCreate) {
      const id = Date.now();
      const coverUrl =
        this.newVehicle.images.length > 0
          ? URL.createObjectURL(this.newVehicle.images[0])
          : '';

      const vehicle: Vehicle = {
        id,
        make: this.getBrandNameById(this.newVehicle.brandId) ?? '',
        model: this.newVehicle.model.trim(),
        year: Number(this.newVehicle.year) || new Date().getFullYear(),
        condition: this.newVehicle.condition,
        fuelType: this.newVehicle.fuelType,
        transmission: this.newVehicle.transmission,
        hp: Number(this.newVehicle.hp) || 0,
        miles: Number(this.newVehicle.miles) || 0,
        price: Number(this.newVehicle.price) || 0,
        status: this.newVehicle.status,
        
        imageUrl: coverUrl,
        images: coverUrl ? [coverUrl] : [],
        engine: this.newVehicle.engine?.trim(),
        drivetrain: this.newVehicle.drivetrain?.trim(),
        mpg: this.newVehicle.mpg?.trim(),
        seating: Number(this.newVehicle.seating) || 5,
        exteriorColor: this.newVehicle.exteriorColor?.trim(),
        interiorColor: this.newVehicle.interiorColor?.trim(),
        description: this.newVehicle.description?.trim(),
        features: this.parseFeatures(this.newVehicle.features),
      };

      this.vehicles.unshift(vehicle);
      this.rebuildFilterOptions();
      this.applyFilters();
      this.closeWizard();
      this.showSuccessDialog('Vehicle added successfully', 'The vehicle has been successfully added to the inventory.');
      return;
    }

    if (this.newVehicle.images.length > 0) {
      const fd = new FormData();
      this.newVehicle.images.forEach(f => fd.append('Files', f));

      this.isLoading = true;
      this.carsApi.uploadImages(fd).subscribe({
        next: res => {
          const urls = Array.isArray((res as any)?.imageUrls)
            ? (res as any).imageUrls
            : Array.isArray((res as any)?.ImageUrls)
              ? (res as any).ImageUrls
              : [];
          const primaryIdx = Math.min(this.primaryUploadIndex, Math.max(urls.length - 1, 0));
          const primary = urls[primaryIdx] || 'placeholder.jpg';
          const gallery = urls.filter((_url: string, i: number) => i !== primaryIdx);
          this.submitCreate(api, primary, gallery);
        },
        error: err => {
          console.error(err);
          this.isLoading = false;
          this.loadError = 'Failed to upload images.';
        }
      });
    } else {
      this.submitCreate(api, 'placeholder.jpg', []);
    }
  }

  private submitCreate(api: any, primaryUrl: string, galleryUrls: string[]): void {
    const payload = this.mapWizardToCreatePayload(this.newVehicle, primaryUrl, galleryUrls);

    this.isLoading = true;
    api.create(payload)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.showSuccessDialog('Vehicle added successfully', 'The vehicle has been successfully added to the inventory.');
          this.closeWizard();
          this.loadCars();
        },
        error: (err: any) => {
          console.error(err);
          this.loadError = 'Failed to add vehicle.';
        }
      });
  }

  updateVehicle(): void {
    if (!this.isEditMode || this.editingVehicleId == null) return;

    const api: any = this.carsApi as any;
    const hasUpdate = api && typeof api.update === 'function';

    this.formErrors = this.validateForm(this.newVehicle);
    if (this.formErrors.length > 0) {
      this.showValidationDialog();
      return;
    }

    if (!hasUpdate) {
      const idx = this.vehicles.findIndex(x => x.id === this.editingVehicleId);
      if (idx < 0) return;

      const current = this.vehicles[idx];
      const coverUrl =
        this.newVehicle.images.length > 0
          ? URL.createObjectURL(this.newVehicle.images[0])
          : current.imageUrl;

      const updated: Vehicle = {
        ...current,
        make: this.getBrandNameById(this.newVehicle.brandId) ?? current.make,
        model: this.newVehicle.model.trim(),
        year: Number(this.newVehicle.year) || current.year,
        price: Number(this.newVehicle.price) || 0,

        engine: this.newVehicle.engine?.trim(),
        hp: Number(this.newVehicle.hp) || 0,
        transmission: this.newVehicle.transmission,
        drivetrain: this.newVehicle.drivetrain?.trim(),
        fuelType: this.newVehicle.fuelType,
        mpg: this.newVehicle.mpg?.trim(),

        miles: Number(this.newVehicle.miles) || 0,
        condition: this.newVehicle.condition,
        seating: Number(this.newVehicle.seating) || 5,
        exteriorColor: this.newVehicle.exteriorColor?.trim(),
        interiorColor: this.newVehicle.interiorColor?.trim(),

        description: this.newVehicle.description?.trim(),
        features: this.parseFeatures(this.newVehicle.features),
        status: this.newVehicle.status,
        imageUrl: coverUrl,
        images: coverUrl ? [coverUrl, ...(current.images ?? [])] : (current.images ?? []),
      };

      this.vehicles[idx] = updated;
      if (this.selectedVehicle?.id === updated.id) this.selectedVehicle = updated;

      this.rebuildFilterOptions();
      this.applyFilters();
      this.closeWizard();
      this.showSuccessDialog('Vehicle updated successfully', 'The vehicle details have been successfully updated.');
      return;
    }

    const payload: UpdateCarRequest = {
      ...this.mapWizardToCreatePayload(
        this.newVehicle,
        this.editingPrimaryImageUrl || undefined,
        this.editingGalleryUrls
      ),
      id: this.editingVehicleId,
      galleryImageUrls: this.editingGalleryUrls
    };

    if (this.newVehicle.images.length > 0) {
      const fd = new FormData();
      this.newVehicle.images.forEach(f => fd.append('Files', f));

      this.isLoading = true;
      this.carsApi.uploadImages(fd).subscribe({
        next: res => {
          const urls = Array.isArray((res as any)?.imageUrls)
            ? (res as any).imageUrls
            : Array.isArray((res as any)?.ImageUrls)
              ? (res as any).ImageUrls
              : [];
          const primaryIdx = Math.min(this.primaryUploadIndex, Math.max(urls.length - 1, 0));
          const primary = urls[primaryIdx] || this.editingPrimaryImageUrl || 'placeholder.jpg';
          const gallery = urls.filter((_url: string, i: number) => i !== primaryIdx);
          this.submitUpdate(api, { ...payload, primaryImageUrl: primary, galleryImageUrls: gallery });
        },
        error: err => {
          console.error(err);
          this.isLoading = false;
          this.loadError = 'Failed to upload images.';
        }
      });
    } else {
      this.submitUpdate(api, payload);
    }
  }

  private submitUpdate(api: any, payload: UpdateCarRequest): void {
    this.isLoading = true;
    api.update(payload)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: () => {
          this.showSuccessDialog('Vehicle updated successfully', 'The vehicle details have been successfully updated.');
          this.closeWizard();
          this.loadCars();
        },
        error: (err: any) => {
          console.error(err);
          this.loadError = 'Failed to update vehicle.';
        }
      });
  }

  private parseFeatures(raw: string): string[] {
    return raw ? raw.split(',').map(x => x.trim()).filter(Boolean) : [];
  }

  private mapListItemToVehicle(item: any): Vehicle {
    const cover = this.normalizeImageUrl(item?.primaryImageUrl ?? '');
    return {
      id: Number(item?.id) || 0,
      make: String(item?.make ?? item?.brandName ?? '-'),
      model: String(item?.model ?? '-'),
      year: Number(item?.year ?? item?.productionYear) || new Date().getFullYear(),
      condition: ((item?.condition as VehicleCondition) ?? 'Used'),
      fuelType: String(item?.fuelType ?? '-'),
      transmission: String(item?.transmission ?? '-'),
      hp: Number(item?.horsePower) || 0,
      miles: Number(item?.mileage) || 0,
      price: Number(item?.discountedPrice ?? item?.price ?? 0) || 0,
      status: this.mapBackendStatusToUi(item?.carStatusName ?? item?.status),
      categoryName:
      item?.categoryName ??
      item?.category?.name ??
      item?.category ??
      (item?.categoryId ? `#${item.categoryId}` : ''),
      imageUrl: cover || '',
      images: cover ? [cover] : [],
      _raw: undefined
    };
  }

  private mapDetailsDtoToVehicle(dto: CarDetailsDto): Vehicle {
    const primary = this.normalizeImageUrl((dto as any).primaryImageUrl ?? '');
    const gallery = Array.isArray((dto as any).imageUrls) ? (dto as any).imageUrls : [];

    const allImages = [
      primary,
      ...gallery.map((x: any) => this.normalizeImageUrl(String(x ?? '')))
    ].filter((x: string) => !!x);

    const uniqueImages = Array.from(new Set(allImages));
    const cover = uniqueImages[0] ?? '';

    return {
      id: Number((dto as any).id) || 0,
      make: (dto as any).brandName ?? (dto as any).make ?? '-',
      model: (dto as any).model ?? '-',
      year: (dto as any).productionYear ?? new Date().getFullYear(),
      condition: ((dto as any).condition as VehicleCondition) ?? 'Used',
      fuelType: (dto as any).fuelType ?? '-',
      transmission: (dto as any).transmission ?? '-',
      hp: this.extractNumber((dto as any).horsePower),
      miles: (dto as any).mileage ?? 0,
      price: (dto as any).discountedPrice ?? (dto as any).price ?? 0,
      status: this.mapBackendStatusToUi((dto as any).carStatusName),
      categoryName:
      (dto as any).categoryName ??
      (dto as any).category?.name ??
      (dto as any).category ??
      ((dto as any).categoryId ? `#${(dto as any).categoryId}` : ''),
      imageUrl: cover,
      images: uniqueImages,
      engine: (dto as any).engine ?? undefined,
      drivetrain: (dto as any).drivetrain ?? undefined,
      mpg: (dto as any).epaFuelEconomy ?? undefined,
      seating: (dto as any).seats ?? undefined,
      doors: (dto as any).doors ?? undefined,
      stockNumber: (dto as any).stockNumber ?? undefined,
      inventoryLocation: (dto as any).inventoryLocation ?? undefined,
      quantityInStock: (dto as any).quantityInStock ?? undefined,
      msrp: (dto as any).msrp ?? null,
      exteriorColor: (dto as any).color ?? undefined,
      interiorColor: (dto as any).interiorColor ?? undefined,
      description: (dto as any).description ?? undefined,
      features: Array.isArray((dto as any).features) ? (dto as any).features : [],
      _raw: dto
    };
  }

  private normalizeImageUrl(url: string): string {
    const u = (url ?? '').trim();
    if (!u) return '';
    if (u.startsWith('http://') || u.startsWith('https://')) return u;
    const path = u.startsWith('/') ? u : `/${u}`;
    return `${environment.apiUrl}${path}`;
  }

  private extractNumber(input: string | number | null | undefined): number {
    if (input == null) return 0;
    if (typeof input === 'number') return input;
    const m = String(input).match(/\d+/);
    return m ? Number(m[0]) : 0;
  }

  private mapBackendStatusToUi(name: string | null | undefined): VehicleStatus {
    const s = (name || '').toLowerCase();
    if (s.includes('sold')) return 'Sold';
    if (s.includes('limit')) return 'Limited';
    return 'In Stock';
  }

  private createEmptyForm(): NewVehicleForm {
    return {
      brandId: 0,
      categoryId: 0,
      carStatusId: 0,
      vin: '',
      stockNumber: '',
      inventoryLocation: '',
      doors: 4,
      quantityInStock: 1,

      make: '',
      model: '',
      year: new Date().getFullYear(),
      price: 0,

      engine: '',
      hp: 0,
      transmission: 'Automatic',
      drivetrain: '',
      fuelType: 'Gasoline',
      mpg: '',

      miles: 0,
      condition: 'Used',
      seating: 5,
      exteriorColor: '',
      interiorColor: '',

      features: '',
      description: '',
      status: 'In Stock',
      images: [],
    };
  }

  

  onCarouselSlide(ev: any): void {
    const to = Number(ev?.to);
    if (!Number.isNaN(to)) this.activeImageIndex = to;
  }

  resetCarouselIndex(): void {
    this.activeImageIndex = 0;
  }

  private mapWizardToCreatePayload(
    form: NewVehicleForm,
    primaryImageUrl?: string,
    galleryImageUrls?: string[]
  ): CreateCarRequest {
    return {
      brandId: form.brandId,
      categoryId: form.categoryId,
      carStatusId: form.carStatusId,

      condition: form.condition,
      stockNumber: form.stockNumber.trim(),
      inventoryLocation: form.inventoryLocation.trim(),
      doors: Number(form.doors) || 4,
      seats: Number(form.seating) || 5,
      quantityInStock: Number(form.quantityInStock) || 1,

      model: form.model.trim(),
      vin: form.vin.trim(),
      productionYear: Number(form.year) || new Date().getFullYear(),
      mileage: Number(form.miles) || 0,

      color: form.exteriorColor?.trim() || 'N/A',
      transmission: form.transmission,
      fuelType: form.fuelType,
      drivetrain: form.drivetrain?.trim() || '',
      engine: form.engine?.trim() || '',
      horsePower: Number(form.hp) || 0,

      epaFuelEconomy: form.mpg?.trim() || null,
      msrp: null,
      description: form.description?.trim() || '',
      price: Number(form.price) || 0,
      discountedPrice: null,
      dateAdded: new Date().toISOString(),

      primaryImageUrl: primaryImageUrl ?? form.images[0]?.name ?? 'placeholder.jpg',
      galleryImageUrls: galleryImageUrls ?? this.mapGalleryImages(form.images),
      features: this.parseFeatures(form.features),
    };
  }

  private mapGalleryImages(files: File[]): string[] {
    return files.slice(1).map(f => f.name);
  }

  private getBrandNameById(id: number): string | undefined {
    return this.brandOptions.find(b => b.id === id)?.name;
  }

  private validateForm(form: NewVehicleForm): string[] {
    const errors: string[] = [];

    if (!form.brandId) errors.push('Brand is required.');
    if (!form.categoryId) errors.push('Category is required.');
    if (!form.carStatusId) errors.push('Status is required.');
    if (!form.model?.trim()) errors.push('Model is required.');
    if (!form.vin?.trim()) errors.push('VIN is required.');
    if (!form.stockNumber?.trim()) errors.push('Stock number is required.');
    if (!form.inventoryLocation?.trim()) errors.push('Inventory location is required.');
    if (!form.fuelType?.trim()) errors.push('Fuel type is required.');
    if (!form.transmission?.trim()) errors.push('Transmission is required.');
    if (!form.engine?.trim()) errors.push('Engine is required.');
    if ((Number(form.year) || 0) < 1900) errors.push('Year must be valid.');
    if ((Number(form.price) || 0) <= 0) errors.push('Price must be greater than 0.');
    if ((Number(form.hp) || 0) <= 0) errors.push('Horsepower must be greater than 0.');
    if ((Number(form.doors) || 0) <= 0) errors.push('Doors must be greater than 0.');
    if ((Number(form.quantityInStock) || 0) <= 0) errors.push('Quantity must be greater than 0.');
    if (!form.description?.trim()) errors.push('Description is required.');

    return errors;
  }

  getFieldError(field: string): string | null {
    const f = this.newVehicle;
    switch (field) {
      case 'brandId':
        return f.brandId ? null : 'Brand is required.';
      case 'categoryId':
        return f.categoryId ? null : 'Category is required.';
      case 'carStatusId':
        return f.carStatusId ? null : 'Status is required.';
      case 'model':
        return f.model?.trim() ? null : 'Model is required.';
      case 'vin': {
        const v = f.vin?.trim() ?? '';
        if (!v) return 'VIN is required.';
        if (v.length !== 17) return 'VIN must be 17 characters.';
        return null;
      }
      case 'stockNumber':
        return f.stockNumber?.trim() ? null : 'Stock number is required.';
      case 'inventoryLocation':
        return f.inventoryLocation?.trim() ? null : 'Inventory location is required.';
      case 'year':
        return (Number(f.year) || 0) >= 1900 ? null : 'Year must be valid.';
      case 'price':
        return (Number(f.price) || 0) > 0 ? null : 'Price must be greater than 0.';
      case 'hp':
        return (Number(f.hp) || 0) > 0 ? null : 'Horsepower must be greater than 0.';
      case 'doors':
        return (Number(f.doors) || 0) > 0 ? null : 'Doors must be greater than 0.';
      case 'quantityInStock':
        return (Number(f.quantityInStock) || 0) > 0 ? null : 'Quantity must be greater than 0.';
      case 'description':
        return f.description?.trim() ? null : 'Description is required.';
      default:
        return null;
    }
  }

  isInvalid(field: string): boolean {
    return !!this.getFieldError(field);
  }

  private showValidationDialog(): void {
    this.dialog.open(FitConfirmDialogComponent, {
      width: '420px',
      data: <DialogConfig>{
        type: DialogType.WARNING,
        title: 'Please fix the form',
        message: this.formErrors.join('\n'),
        buttons: [{ type: DialogButton.OK, label: 'Close' }]
      }
    });
  }

  private mapDetailsDtoToForm(dto: CarDetailsDto): NewVehicleForm {
    return {
      brandId: (dto as any).brandId ?? 0,
      categoryId: (dto as any).categoryId ?? 0,
      carStatusId: (dto as any).carStatusId ?? 0,
      vin: (dto as any).vin ?? '',
      stockNumber: (dto as any).stockNumber ?? '',
      inventoryLocation: (dto as any).inventoryLocation ?? '',
      doors: (dto as any).doors ?? 4,
      quantityInStock: (dto as any).quantityInStock ?? 1,

      make: (dto as any).brandName ?? '',
      model: (dto as any).model ?? '',
      year: (dto as any).productionYear ?? new Date().getFullYear(),
      price: (dto as any).price ?? (dto as any).discountedPrice ?? 0,

      engine: (dto as any).engine ?? '',
      hp: (dto as any).horsePower ?? 0,
      transmission: (dto as any).transmission ?? 'Automatic',
      drivetrain: (dto as any).drivetrain ?? '',
      fuelType: (dto as any).fuelType ?? 'Gasoline',
      mpg: (dto as any).epaFuelEconomy ?? '',

      miles: (dto as any).mileage ?? 0,
      condition: (dto as any).condition as VehicleCondition ?? 'Used',
      seating: (dto as any).seats ?? 5,
      exteriorColor: (dto as any).color ?? '',
      interiorColor: (dto as any).interiorColor ?? '',

      features: Array.isArray((dto as any).features) ? (dto as any).features.join(', ') : '',
      description: (dto as any).description ?? '',
      status: this.mapBackendStatusToUi((dto as any).carStatusName),
      images: [],
    };
  }

  private syncPaginatorVm(): void {
    this.paginatorVm.request.paging.page = this.page;
    this.paginatorVm.request.paging.pageSize = this.pageSize;
    this.paginatorVm.totalItems = this.totalCount;
    this.paginatorVm.totalPages = this.totalPages;
    this.paginatorVm.isLoading = this.isLoading;
  }

  setActiveImage(i: number): void {
    if (!this.selectedVehicle?.images?.length) return;
    const max = this.selectedVehicle.images.length - 1;
    this.activeImageIndex = Math.min(Math.max(i, 0), max);
  }

  nextImage(): void {
    if (!this.selectedVehicle?.images?.length) return;
    this.activeImageIndex = (this.activeImageIndex + 1) % this.selectedVehicle.images.length;
  }

  prevImage(): void {
    if (!this.selectedVehicle?.images?.length) return;
    this.activeImageIndex =
      (this.activeImageIndex - 1 + this.selectedVehicle.images.length) % this.selectedVehicle.images.length;
  }

  openZoom(url?: string, ev?: MouseEvent): void {
  if (!url) return;
  
  if (this.zoomedImage === url) {
    this.closeZoom();
    return;
  }
  if (ev && ev.target instanceof HTMLElement) {
    const rect = ev.target.getBoundingClientRect();
    const x = ((ev.clientX - rect.left) / rect.width) * 100;
    const y = ((ev.clientY - rect.top) / rect.height) * 100;
    this.zoomOrigin = `${x}% ${y}%`;
  } else {
    this.zoomOrigin = 'center center';
  }
  this.zoomedImage = url;
}

closeZoom(): void {
  this.zoomedImage = null;
}

onHeroMouseMove(ev: MouseEvent, imgUrl?: string): void {
  if (!imgUrl) return;
  const target = ev.target as HTMLElement;
  const rect = target.getBoundingClientRect();
  const x = ev.clientX - rect.left;
  const y = ev.clientY - rect.top;
  const clampedX = Math.max(0, Math.min(x, rect.width));
  const clampedY = Math.max(0, Math.min(y, rect.height));
  this.lensBgImage = imgUrl;
  this.lensBgPos = `${(clampedX / rect.width) * 100}% ${(clampedY / rect.height) * 100}%`;
  this.lensX = clampedX - this.lensSize / 2;
  this.lensY = clampedY - this.lensSize / 2;
  this.lensVisible = true;
}

onHeroMouseLeave(): void {
  this.lensVisible = false;
}

}
