import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {config} from '../../../config';
import {AuthUser} from '../models/auth.user';
import {Observable, of} from 'rxjs';
import {catchError, mapTo, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly TOKEN = 'TOKEN';
  private loggedUser: AuthUser;

  constructor(private http: HttpClient) {}

  register(user: {email: string, password: string, confirmPassword: any }): Observable<boolean> {
    return this.http.post<any>(`${config.apiUrl}/api/account/register`, user)
      .pipe(
        mapTo(true),
        catchError((error: HttpErrorResponse) => {
          alert(error.error['message']);
          return of(false);
        }));
  }

  logIn(user: { userName: string, password: string }) {
    const url = `${config.apiUrl}/Token`;
    const body = new URLSearchParams();
    body.set('userName', user.userName);
    body.set('password', user.password);
    body.set('grant_type', 'password');

    const authheaders = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
    });

    return this.http.post(url, body.toString(), {headers: authheaders})
      .pipe(
      tap(data => this.doLoginUser(data)),
      mapTo(true),
      catchError((error: HttpErrorResponse) => {
        alert(error.error['error_description']);
        return of(false);
      }));
  }


  logout() {
    this.doLogoutUser();
  }

  isLoggedIn(): boolean {
    const b: boolean = !!this.getToken();
    return b;
  }

  getToken() {
    return localStorage.getItem(this.TOKEN);
  }
  getUser() {
    return this.loggedUser;
  }
  private doLoginUser(data) {
    const token = data.access_token;
    this.loggedUser =
    {
      name: data.userName,
      role: data.userRole,
    };
    console.log(this.loggedUser);
    this.storeToken(token);
  }

  private doLogoutUser() {
    this.loggedUser = null;
    this.removeTokens();
  }


  private storeToken(token: string) {
    localStorage.setItem(this.TOKEN, token);
  }

  private removeTokens() {
    localStorage.removeItem(this.TOKEN);
  }


}
