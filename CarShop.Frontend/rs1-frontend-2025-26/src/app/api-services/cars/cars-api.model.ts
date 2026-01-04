// ==========================
// READ MODELS (GET)
// ==========================
export interface CarDetailsDto {
  id: number;

  brandId: number;
  brandName: string;

  categoryId: number;
  categoryName: string;

  carStatusId: number;
  carStatusName: string;


  condition: string;              
  stockNumber: string;
  inventoryLocation: string;
  doors: number;
  seats: number;

  model: string;
  vin: string;
  productionYear: number;
  mileage: number;

  color: string;
  transmission: string;
  fuelType: string;
  drivetrain: string;
  engine: string;
  horsePower: number;

  epaFuelEconomy?: string | null;
  msrp?: number | null;
  quantityInStock: number;

  description?: string | null;
  price: number;
  discountedPrice?: number | null;
  dateAdded: string;


  primaryImageUrl?: string | null;
  imageUrls: string[];
  features: string[];

  createdAtUtc: string;
  modifiedAtUtc?: string | null;
}



// ==========================
// WRITE MODELS (POST/PUT)
// ==========================
export interface CreateCarRequest {
  brandId: number;
  categoryId: number;
  carStatusId: number;


  condition: string;
  stockNumber: string;
  inventoryLocation: string;
  doors: number;
  seats: number;

  model: string;
  vin: string;
  productionYear: number;
  mileage: number;

  color: string;
  transmission: string;
  fuelType: string;
  drivetrain: string;
  engine: string;
  horsePower: number;

  epaFuelEconomy?: string | null;
  msrp?: number | null;
  quantityInStock: number;

  description?: string | null;
  price: number;
  discountedPrice?: number | null;
  dateAdded?: string | null;


  primaryImageUrl: string;
  galleryImageUrls?: string[] | null;
  features?: string[] | null;
}

export interface UpdateCarRequest extends CreateCarRequest {
  id: number;
  galleryImageUrls?: string[] | null;


}

export interface DeleteCarRequest {
  id: number;
}

export interface PagedResult<T> {
  totalCount: number;
  page: number;
  pageSize: number;
  items: T[];
}

// ============ UPLOAD ============
export interface UploadCarImagesResponseDto {
  imageUrls: string[];
}

