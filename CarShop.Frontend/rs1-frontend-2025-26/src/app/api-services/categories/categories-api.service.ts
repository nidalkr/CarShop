import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LookupItemDto } from './categories-api.model';

@Injectable({ providedIn: 'root' })
export class CategoriesApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/categories`;
  constructor(private http: HttpClient) {}

  list(search?: string): Observable<LookupItemDto[]> {
    const params = search ? { search } : undefined;
    return this.http.get<LookupItemDto[]>(this.baseUrl, { params });
  }
}
