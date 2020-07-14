import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {Author} from '../models/author';
import {BookCard} from '../models/book-card';
import {BookService} from './bookService';

@Injectable()
export class AuthorService {

  private url = config.apiUrl + '/api/authors';

  constructor(private http: HttpClient, private bookService: BookService) {
  }

  getAuthors(): Observable<Author[]>{
    return this.http.get<Author[]>(this.url).pipe(map(
      (authors: Author[]) =>    authors
    ));
  }

  searchAuthors(name: string): Observable<Author[]>{
    const searchUrl = `${this.url}/search/${name}`;
    return this.http.get<Author[]>(searchUrl).pipe(map(
      (authors: Author[]) =>    authors
    ));
  }

  getBookByAuthor(id: number): Observable<BookCard[]>{
    const url = `${this.url}/${id}/books`;
    return this.bookService.getBooks(url);
  }

  getAuthor(id: number): Observable<Author> {
    return this.http.get<Author>(this.url + '/' + id).pipe(map(
      (author: Author) =>    author
    ));
  }

  createAuthor(author: Author) {
    return this.http.post(this.url, author);
  }
  updateAuthor(author: Author) {

    return this.http.put(this.url, author);
  }
  deleteAuthor(id: number) {
    return this.http.delete(this.url + '/' + id);
  }

}
