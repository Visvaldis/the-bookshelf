import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {BookCard} from '../models/book-card';
import {BookService} from './bookService';
import {AuthUser} from '../app/auth/models/auth.user';

@Injectable()
export class UserService {


  private url = config.apiUrl + '/api/account';

  constructor(private http: HttpClient, private bookService: BookService) {
  }

  getUsers(): Observable<AuthUser[]>{
    return this.http.get<AuthUser[]>(this.url).pipe(map(
      (users: AuthUser[]) =>    users
    ));
  }

  deleteUser(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
  getLikedBooks(): Observable<BookCard[]> {
    const url = `${this.url}/likedbooks`;
    return this.bookService.getBooks(url);
  }


  likeBook(id: number): Observable<any> {
    const url =  this.url + '/like/' + id;
    return this.http.get(url, { observe: 'response', responseType: 'json'});
  }


}
