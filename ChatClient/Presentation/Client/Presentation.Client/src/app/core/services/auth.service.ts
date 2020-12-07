import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiError, AuthenticatedUser, CreateAccountCredentials, LoginCredentials } from '@chat-client/core/models';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private readonly httpClient: HttpClient) { }

  authenticate(): Observable<AuthenticatedUser> {
    const url = `${environment.api.users}/me`;

    return this.httpClient.get<AuthenticatedUser>(url);
  }

  login(credentials: LoginCredentials): Observable<AuthenticatedUser> {
    const url = `${environment.api.session}`;

    return this.httpClient.put<AuthenticatedUser>(url, credentials);
  }

  createAccount(credentials: CreateAccountCredentials): Observable<void> {
    const url = `${environment.api.users}`;

    return this.httpClient.post<void>(url, credentials);
  }

  emailExists(email: string): Observable<boolean> {
    const url = `${environment.api.users}/email`;

    const options = {
      params: new HttpParams().set('email', email)
    };

    // Map status code '200' => true and '404' => false
    return this.httpClient.head(url, options).pipe(
      map(() => true),
      catchError((error: ApiError) => {
        return error.statusCode === 404 ? of(false) : throwError(error);
      })
    );
  }

  userNameExists(userName: string): Observable<boolean> {
    const url = `${environment.api.users}/name`;

    const options = {
      params: new HttpParams().set('userName', userName)
    };

    // Map status code '200' => true and '404' => false
    return this.httpClient.head(url, options).pipe(
      map(() => true),
      catchError((error: ApiError) => {
        return error.statusCode === 404 ? of(false) : throwError(error);
      })
    );
  }
}
