import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Tag} from '../models/tag';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {BookCard} from '../models/book-card';
import {BookService} from './bookService';

@Injectable()
export class UserService {


  private url = config.apiUrl + '/api/account';

  constructor(private http: HttpClient, private bookService: BookService) {
  }

  getUsers(): Observable<Tag[]>{
    return this.http.get<Tag[]>(this.url).pipe(map(
      (tags: Tag[]) =>    tags
    ));
  }

  getLikedBooks(): Observable<BookCard[]> {
    const url = `${this.url}/likedbooks`;
    return this.bookService.getBooks(url);
  }

  changePassword(){

}

  likeBook(id: number): Observable<any> {
    const url =  this.url + '/like/' + id;
    return this.http.get(url, { observe: 'response', responseType: 'json'});
  }


}
