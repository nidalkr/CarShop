import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StatusLookupDto } from './statuses-api.model';

@Injectable({ providedIn: 'root' })
export class StatusesApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/statuses`;
  constructor(private http: HttpClient) {}

  list(search?: string): Observable<StatusLookupDto[]> {
    const params = search ? { search } : undefined;
    return this.http.get<StatusLookupDto[]>(this.baseUrl, { params });
  }
}
