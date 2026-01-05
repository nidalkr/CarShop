import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CarDetailsDto, CreateCarRequest, PagedResult, UpdateCarRequest, UploadCarImagesResponseDto } from './cars-api.model';

@Injectable({ providedIn: 'root' })
export class CarsApiService {
  
  private readonly baseUrl = `${environment.apiUrl}/api/cars`;

  constructor(private http: HttpClient) {}

  getById(id: number): Observable<CarDetailsDto> {
    return this.http.get<CarDetailsDto>(`${this.baseUrl}/${id}`);
  }

  getAll(page = 1, pageSize = 10) {
    return this.http.get<PagedResult<CarDetailsDto>>(
      `${this.baseUrl}?page=${page}&pageSize=${pageSize}`
    );
  }

  create(req: CreateCarRequest): Observable<CarDetailsDto> {
    return this.http.post<CarDetailsDto>(this.baseUrl, req);
  }

  update(req: UpdateCarRequest): Observable<CarDetailsDto> {
    return this.http.put<CarDetailsDto>(`${this.baseUrl}/${req.id}`, req);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  uploadImages(formData: FormData): Observable<UploadCarImagesResponseDto> {
    return this.http.post<UploadCarImagesResponseDto>(`${this.baseUrl}/images`, formData);
  }
}
